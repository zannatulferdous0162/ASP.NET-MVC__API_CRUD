using EvidencePracticeMidMonth07.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace EvidencePracticeMidMonth07.Controllers
{
    public class OrderController : ApiController
    {
        private MyDbContext _db = new MyDbContext();

        public IHttpActionResult GetOrder()
        {
            var data = _db.OrderMasters.ToList();

            var jsonset = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            var serial = JsonConvert.SerializeObject(data, Formatting.None, jsonset);
            var jsonObj = JsonConvert.DeserializeObject(serial);

            return Ok(jsonObj);

        }

        public IHttpActionResult GetOrder(int id)
        {
            OrderMaster order = _db.OrderMasters.FirstOrDefault(o => o.OrderId == id);

            var jsonset = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            var serial = JsonConvert.SerializeObject(order, Formatting.None, jsonset);
            var jsonObj = JsonConvert.DeserializeObject(serial);

            return Ok(jsonObj);
        }


        public IHttpActionResult PostOrder()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var order = new OrderMaster()
            {
                CustomerName = HttpContext.Current.Request.Form["CustomerName"],
                OrderDate = string.IsNullOrEmpty(HttpContext.Current.Request.Form["OrderDate"])
                    ? DateTime.Now 
                    : Convert.ToDateTime(HttpContext.Current.Request.Form["OrderDate"]),
                IsComplete = string.IsNullOrEmpty(HttpContext.Current.Request.Form["IsComplete"])
                     ? (bool?)null
                     : Convert.ToBoolean(HttpContext.Current.Request.Form["IsComplete"])
            };

            var ImageFile = HttpContext.Current.Request.Files["ImageFile"];
            if (ImageFile != null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetFileName(ImageFile.FileName);
                var imagepath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images"), filename);
                order.ImagePath = imagepath;
                ImageFile.SaveAs(imagepath);
            }

            var detail = HttpContext.Current.Request.Form["OrderDetail"];
            //if (detail != null)
            //{
            //    var ordList = JsonConvert.DeserializeObject<List<OrderDetail>>(detail);
            //    foreach (var item in ordList)
            //    {
            //        item.OrderId = order.OrderId;
            //        _db.Entry(item).State = EntityState.Added;
            //    }
            //    order.OrderDetail = ordList;
            //}
            if (!string.IsNullOrEmpty(detail))
            {
                var ordList = JsonConvert.DeserializeObject<List<OrderDetail>>(detail);
                order.OrderDetail = ordList;
            }
            _db.OrderMasters.Add(order);
            _db.SaveChanges();
            return Ok();
        }


        public IHttpActionResult PutOrder(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var order = _db.OrderMasters.Include(o => o.OrderDetail).FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            order.CustomerName = HttpContext.Current.Request.Form["CustomerName"];
            order.OrderDate = string.IsNullOrEmpty(HttpContext.Current.Request.Form["OrderDate"])
                  ? order.OrderDate
                  : Convert.ToDateTime(HttpContext.Current.Request.Form["OrderDate"]);
            order.IsComplete = string.IsNullOrEmpty(HttpContext.Current.Request.Form["IsComplete"])
                               ? order.IsComplete
                               : Convert.ToBoolean(HttpContext.Current.Request.Form["IsComplete"]);

            var ImageFile = HttpContext.Current.Request.Files["ImageFile"];

            if (ImageFile != null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetFileName(ImageFile.FileName);
                var imagepath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images"), filename);
                order.ImagePath = imagepath;
                ImageFile.SaveAs(imagepath);
            }

            var detail = HttpContext.Current.Request.Form["OrderDetail"];
            if (!string.IsNullOrEmpty(detail))
            {
                var ordList = JsonConvert.DeserializeObject<List<OrderDetail>>(detail);
                //order.OrderDetail.Clear();
                //order.OrderDetail.AddRange(ordList);
                _db.OrderDetails.RemoveRange(order.OrderDetail);
                _db.SaveChanges();
                foreach (var item in ordList)
                {
                    item.OrderId = order.OrderId;
                    _db.OrderDetails.Add(item);
                }
                _db.SaveChanges();
            }
            _db.Entry(order).State = EntityState.Modified;
            _db.SaveChanges();
            return Ok();
        }


        public IHttpActionResult DeleteOrder(int id)
        {
            var order = _db.OrderMasters.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            _db.OrderMasters.Remove(order);
            _db.SaveChanges();
            return Ok();
        }
    }
}
