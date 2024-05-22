namespace backend_lembrol.Utils
{
    public static class ValidationUtils
    {
        public static void ValidateIntDayOfWeek(int dayDto)
        {
            if (dayDto < 0 || dayDto >= 7)
            {
                throw new ArgumentOutOfRangeException(nameof(dayDto), dayDto, "Day must be between 0 and 6.");
            }
        }
    }
}
