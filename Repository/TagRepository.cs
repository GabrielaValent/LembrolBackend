using Microsoft.EntityFrameworkCore;
using backend_lembrol.Entity;
using backend_lembrol.Dto;
using backend_lembrol.Database;
using backend_lembrol.Utils;


namespace backend_lembrol.Repository
{
    public class TagRepository
    {
        private readonly DataContext _context;

        public TagRepository(DataContext context)
        {
            _context = context;
        }

        public void CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);
        }

        public void AddDayOfWeek(List<DaysOfWeek> days)
        {
            _context.DaysOfWeek.AddRange(days);
        }

        public void AddSpecificDate(List<SpecificDates> sDays)
        {
            _context.SpecificDates.AddRange(sDays);
        }

        public async Task<List<CompleteTagDto>> GetTags()
        {
            var tags = await _context.Tags.ToListAsync();

            var tagDtos = new List<CompleteTagDto>();

            foreach (var tag in tags)
            {
                var daysOfWeek = await _context.DaysOfWeek.Where(d => d.TagId == tag.TagId).ToListAsync();
                var specificDates = await _context.SpecificDates.Where(s => s.TagId == tag.TagId).ToListAsync();

                var daysOfWeekDto = daysOfWeek.Select(d => new DaysOfWeekDto
                {
                    TagId = d.TagId,
                    Day = d.DayOfWeek,
                    Active = d.Active
                    
                }).ToList();

                var specificDatesDto = specificDates.Select(s => new SpecificDatesDto
                {
                    TagId = s.TagId,
                    Date = s.SpecificDate,
                    Active = s.Active
                }).ToList();

                tagDtos.Add(new CompleteTagDto
                {
                    TagId = tag.TagId,
                    Name = tag.Name,
                    Active = tag.Active,
                    Color = tag.Color,
                    DaysOfWeek = daysOfWeekDto,
                    SpecificDates = specificDatesDto
                });
            }

            return tagDtos;
        }

        public async Task<CompleteTagDto> GetTagById(string id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null) { throw new Exception("TagId not Found"); }

            var daysOfWeek = await _context.DaysOfWeek.Where(d => d.TagId == tag.TagId).ToListAsync();
            var specificDates = await _context.SpecificDates.Where(s => s.TagId == tag.TagId).ToListAsync();

            var daysOfWeekDto = daysOfWeek.Select(d => new DaysOfWeekDto
            {
                TagId = d.TagId,
                Day = d.DayOfWeek,
                Active = d.Active
            }).ToList();

            var specificDatesDto = specificDates.Select(s => new SpecificDatesDto
            {
                TagId = s.TagId,
                Date = s.SpecificDate,
                Active = s.Active
            }).ToList();

            return new CompleteTagDto
            {
                TagId = tag.TagId,
                Name = tag.Name,
                Color = tag.Color,
                Active = tag.Active,
                DaysOfWeek = daysOfWeekDto,
                SpecificDates = specificDatesDto
            };
        }

        public async void UpdateTag(string id,CompleteUpdateTagDto updatedTag)
        {
            var oldTag = await _context.Tags.FindAsync(id);

            if (oldTag == null) { throw new Exception("TagId not Found"); }

            oldTag.Name = updatedTag.Name;
            oldTag.Active = updatedTag.Active;
            oldTag.Color = updatedTag.Color;
            _context.Tags.Update(oldTag);

            List<DaysOfWeek> daysToUpdate = new List<DaysOfWeek>();
            foreach (var day in updatedTag.DaysOfWeek)
            {
                ValidationUtils.ValidateIntDayOfWeek(day.Day);
                var existingDay = await _context.DaysOfWeek
                    .FirstOrDefaultAsync(d => d.TagId == id && d.DayOfWeek == day.Day);

                if (existingDay != null)
                {
                    existingDay.Active = day.Active; 
                    daysToUpdate.Add(existingDay);
                }
                else
                {
                    DaysOfWeek dayEntity = new()
                    {
                        TagId = id,
                        DayOfWeek = day.Day,
                        Active = day.Active
                    };
                    daysToUpdate.Add(dayEntity);
                }
                
            }

            var daysToCheck = daysToUpdate.Select(up => up.DayOfWeek).ToList();
            var oldDaysOfWeek = await _context.DaysOfWeek.Where(d => d.TagId == oldTag.TagId).ToListAsync();
            var daysToRemove = oldDaysOfWeek.Where(od => !daysToCheck.Contains(od.DayOfWeek));
            _context.DaysOfWeek.RemoveRange(daysToRemove);
            _context.DaysOfWeek.UpdateRange(daysToUpdate);

            

            List<SpecificDates> datesToUpdate = new List<SpecificDates>();
            foreach (var date in updatedTag.SpecificDates)
            {
                var existingDate = await _context.SpecificDates
                    .FirstOrDefaultAsync(d => d.TagId == id && d.SpecificDate == date.Date);

                if (existingDate != null)
                {
                    existingDate.Active = date.Active;
                    datesToUpdate.Add(existingDate);
                }
                else
                {
                    SpecificDates dateEntity = new()
                    {
                        TagId = id,
                        SpecificDate = date.Date,
                        Active = date.Active
                    };
                    datesToUpdate.Add(dateEntity);
                }
            }

            var datesToCheck = datesToUpdate.Select(up => up.SpecificDate).ToList();
            var oldSpecificDates = await _context.SpecificDates.Where(s => s.TagId == oldTag.TagId).ToListAsync();
            var datesToRemove = oldSpecificDates.Where(od => !datesToCheck.Contains(od.SpecificDate));
            _context.SpecificDates.RemoveRange(datesToRemove);
            _context.SpecificDates.UpdateRange(datesToUpdate);

        }

        private int ManageActivate(int active)
        {
            return active == 0 ? 1 : 0;
        }

        public async Task TagState(string tagId)
        {
            var tagEntity = await _context.Tags.FindAsync(tagId);
            if (tagEntity == null) { return; }

            var currentDate = DateTime.UtcNow.AddDays(1);
            var specificDatesEntities = await _context.SpecificDates.Where(s => s.TagId == tagId && s.SpecificDate < currentDate).ToListAsync();
            foreach (var specificDateEntity in specificDatesEntities)
            {
                _context.SpecificDates.Remove(specificDateEntity);
            }

            var daysOfWeekEntities = await _context.DaysOfWeek.Where(d => d.TagId == tagId).ToListAsync();
            foreach (var dayOfWeekEntity in daysOfWeekEntities)
            {
                dayOfWeekEntity.Active = ManageActivate(tagEntity.Active);
                _context.DaysOfWeek.Update(dayOfWeekEntity);
            }

            var specificDatesToReactivate = await _context.SpecificDates.Where(s => s.TagId == tagId && s.SpecificDate >= currentDate).ToListAsync();
            foreach (var specificDateEntity in specificDatesToReactivate)
            {
                specificDateEntity.Active = ManageActivate(tagEntity.Active);
                _context.SpecificDates.Update(specificDateEntity);
            }

            tagEntity.Active = ManageActivate(tagEntity.Active);
            _context.Tags.Update(tagEntity);
        }

        public async Task DaysOfWeekState(string tagId, int dayOfWeek)
        {
            var entity = await _context.DaysOfWeek.FirstOrDefaultAsync(d => d.TagId == tagId && d.DayOfWeek == dayOfWeek);
            if (entity != null)
            {
                entity.Active = ManageActivate(entity.Active);
                _context.DaysOfWeek.Update(entity);
            }
        }

        public async Task DeleteSpecificDate(string tagId, DateTime date)
        {
            var entity = await _context.SpecificDates.FirstOrDefaultAsync(d => d.TagId == tagId && d.SpecificDate == date);
            if (entity != null)
            {
                _context.SpecificDates.Remove(entity);
            }
        }

        public async Task DeleteTag(string tagId)
        {
            var tagEntity = await _context.Tags.FindAsync(tagId);
            if (tagEntity == null){return;}

            var daysOfWeekEntities = await _context.DaysOfWeek.Where(d => d.TagId == tagId).ToListAsync();
            foreach (var dayOfWeekEntity in daysOfWeekEntities)
            {
                _context.DaysOfWeek.Remove(dayOfWeekEntity);
            }

            var specificDatesEntities = await _context.SpecificDates.Where(s => s.TagId == tagId).ToListAsync();
            foreach (var specificDateEntity in specificDatesEntities)
            {
                _context.SpecificDates.Remove(specificDateEntity);
            }

            _context.Tags.Remove(tagEntity);
        }
    }
}