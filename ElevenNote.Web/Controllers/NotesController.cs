using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.Web.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly Lazy<NoteService> _svc;


        public NotesController()
        {
            _svc =
                new Lazy<NoteService>(
                    () =>
                    {
                        var userId = Guid.Parse(User.Identity.GetUserId());
                        return new NoteService(userId);
                    }
                );
        }

        // GET: Notes
        public ActionResult Index()
        {
            var notes = _svc.Value.GetNotes();
            
            return View(notes);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);

     
            if (!_svc.Value.CreateNote(model))
            {
                ModelState.AddModelError("", "Unable to create note");
                return View(model);
            }

            TempData["SaveResult"] = "Your note was created";

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var note = _svc.Value.GetNoteById(id);
            return View(note);
        }
    }
}