using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Filters;
using ProyectoFinal.Models.Repositories;
using System.Configuration;
using PagedList;

namespace ProyectoFinal.Controllers
{
    [AuthorizationPrivilege(Role = "Admin")]
    [HandleError()]
    public class OldGroupsController : Controller
    {
        /*  #region Properties
          private IOldGroupRepository groupRepository;
          #endregion

          #region Constructors
          public OldGroupsController()
          {
              this.groupRepository = new OldGroupRepository(new GymContext());
          }

          public OldGroupsController(IOldGroupRepository groupRepository)
          {
              this.groupRepository = groupRepository;
          }
          #endregion
          // GET: Groups
          public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
          {
              ViewBag.CurrentSort = sortOrder;
              if (searchString != null)
              {
                  page = 1;
              }
              else
              {
                  searchString = currentFilter;
              }

              ViewBag.CurrentFilter = searchString;

              var groups = groupRepository.GetGroups();
              #region search

              if (!String.IsNullOrEmpty(searchString))
              {
                  groups = groups.Where(r => r.GroupID.ToString().ToLower().Contains(searchString.ToLower()));
              }
              #endregion

              #region OrderBy
              ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
              ViewBag.DescSortParm = sortOrder == "description_asc" ? "description_desc" : "description_asc";

              switch (sortOrder)
              {
                  case "name_desc":
                      groups = groups.OrderByDescending(a => a.Name);
                      break;
                  case "description_desc":
                      groups = groups.OrderByDescending(a => a.Description);
                      break;
                  case "description_asc":
                      groups = groups.OrderBy(a => a.Description);
                      break;
                  default:
                      groups = groups.OrderBy(a => a.Name);
                      break;
              }
              #endregion

              int pageNumber = (page ?? 1);
              int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
              return View(groups.ToPagedList(pageNumber, pageSize));
          }

          [AuthorizationPrivilege(Role = "Admin")]
          // GET: Groups/Details/5
          public ActionResult Details(int? id)
          {
              if (id == null)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              OldGroup group = groupRepository.GetGroupByID((int)id);
              if (group == null)
              {
                  return HttpNotFound();
              }
              return View(group);
          }

          [AuthorizationPrivilege(Role = "Admin")]
          // GET: Groups/Create
          public ActionResult Create()
          {
              return View();
          }

          // POST: Groups/Create
          // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
          // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
          [AuthorizationPrivilege(Role = "Admin")]
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Create([Bind(Include = "GroupID,Name,Description,Level,Amount,StartTime,ClosingTime")] OldGroup group)
          {
              group.Amount = 0;
              if (ModelState.IsValid)
              {
                  groupRepository.InsertGroup(group);
                  groupRepository.Save();
                  return RedirectToAction("Index");
              }


              return View(group);
          }



          // GET: Groups/Edit/5
          [AuthorizationPrivilege(Role = "Admin")]
          public ActionResult Edit(int? id)
          {
              if (id == null)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              OldGroup group = groupRepository.GetGroupByID((int)id);
              if (group == null)
              {
                  return HttpNotFound();
              }
              return View(group);
          }

          // POST: Group/Edit/5
          // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
          // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
          [AuthorizationPrivilege(Role = "Admin")]
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Edit([Bind(Include = "GroupID,Name,Description,Level,Quota,Amount")] OldGroup group)
          {
              if (ModelState.IsValid)
              {
                  groupRepository.UpdateGroup(group);
                  groupRepository.Save();
                  return RedirectToAction("Index");
              }
              return View(group);
          }

          // GET: Activities/Delete/5
          [AuthorizationPrivilege(Role = "Admin")]
          public ActionResult Delete(int? id)
          {
              if (id == null)
              {
                  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
              }
              OldGroup group = groupRepository.GetGroupByID((int)id);
              if (group == null)
              {
                  return HttpNotFound();
              }
              return View(group);
          }

          // POST: Groups/Delete/5
          [AuthorizationPrivilege(Role = "Admin")]
          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public ActionResult DeleteConfirmed(int id)
          {
              OldGroup group = groupRepository.GetGroupByID((int)id);
              groupRepository.DeleteGroup((int)id);
              groupRepository.Save();
              return RedirectToAction("Index");
          }

          protected override void Dispose(bool disposing)
          {
              if (disposing)
              {
                  groupRepository.Dispose();
              }
              base.Dispose(disposing);
          }
      }*/
    }
}
