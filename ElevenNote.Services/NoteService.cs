using ElevenNote.Data;
using ElevenNote.Models;
using ElevenNote.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;
        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<NoteListItemModel> GetNotes()
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var query =
                    ctx
                        .Notes
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new NoteListItemModel
                                {
                                    NoteId = e.NoteId,
                                    Title = e.Title,
                                    CreatedUtc = e.CreatedUtc
                                }
                        );

                return query.ToArray();
            }
        }

        //public IEnumerable<NoteListItemModel> GetNotes()
        //{

        //}
        // TODO: Get additional stuff to type out. 


        public bool CreateNote(NoteCreateModel model)
        {
            var entity =
                new NoteEntity
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.UtcNow
                };

            using (var ctx = new ElevenNoteDbContext())
            {
                ctx.Notes.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public NoteDetailModel GetNoteById(int noteId)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity = 
                    ctx
                    .Notes
                    .Single(e => e.NoteId == noteId && e.OwnerId == _userId);
           
                return
                    new NoteDetailModel
                    {
                        NoteId = entity.NoteId,
                        Title = entity.Title,
                        Content = entity.Content,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

    }
}
