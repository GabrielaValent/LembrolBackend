using backend_lembrol.Entity;
using backend_lembrol.Dto;
using backend_lembrol.Repository;
using backend_lembrol.DataAccess.Interfaces;
using backend_lembrol.Utils;

namespace backend_lembrol.Service
{
    public class TagService
    {
        private readonly TagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;


        public TagService(TagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateTag(TagDto dto)
        {
            Tag tagEntity = new()
            {
                TagId = dto.TagId,
                Color = dto.Color,
                Name = dto.Name,
                Active = 1
            };

            List<DaysOfWeek> listDaysOfWeekEntity = new();
            foreach (var dayDto in dto.DaysOfWeek)
            {
                ValidationUtils.ValidateIntDayOfWeek(dayDto);
                DaysOfWeek dayEntity = new()
                {
                    TagId = dto.TagId,
                    DayOfWeek = dayDto,
                    Active = 1
                };
                listDaysOfWeekEntity.Add(dayEntity);
            }

            List<SpecificDates> listSpecificDays = new();
            foreach (var dateDto in dto.SpecificDates)
            {
                SpecificDates sDayEntity = new()
                {
                    TagId = dto.TagId,
                    SpecificDate = dateDto,
                    Active = 1
                };
                listSpecificDays.Add(sDayEntity);
            }

            _tagRepository.CreateTag(tagEntity);
            _tagRepository.AddDayOfWeek(listDaysOfWeekEntity);
            _tagRepository.AddSpecificDate(listSpecificDays);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateTag(string id, CompleteUpdateTagDto dto)
        {
            _tagRepository.UpdateTag(id,dto);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<CompleteTagDto>> GetTags()
        {
            return await _tagRepository.GetTags();
        }

        public async Task<CompleteTagDto> GetTagById(string id)
        {
            return await _tagRepository.GetTagById(id);
        }

        public async Task TagState(string id)
        {
            await _tagRepository.TagState(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteTag(string id)
        {
            await _tagRepository.DeleteTag(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task DayOfWeekState(string id, int dayOfWeek)
        {
            ValidationUtils.ValidateIntDayOfWeek(dayOfWeek);
            await _tagRepository.DaysOfWeekState(id, dayOfWeek);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteSpecificDate(string id, DateTime date)
        {
            await _tagRepository.DeleteSpecificDate(id, date);
            await _unitOfWork.SaveAsync();
        }
    }
}