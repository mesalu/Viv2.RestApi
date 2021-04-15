using System;

namespace Viv2.API.Core.Entities
{
    public class EnvDataSample
    {
        public int Id { get; set; }
        public Environment Environment { get; set; }
        
        private DateTime? _captured;
        public DateTime Captured
        {
            get
            {
                if (_captured.HasValue && _captured.Value.Kind == DateTimeKind.Unspecified)
                    // Part of Core's spec to Infrastructure providers is that all date time values are UTC.
                    // if the infrastructure component is messing up, but including the locale, we can correct
                    // but otherwise we're forced to assume that Kind is UTC.
                    return DateTime.SpecifyKind(_captured.Value, DateTimeKind.Utc);
                
                return _captured.HasValue ? _captured.Value : DateTime.MinValue;
            }
            set => _captured = value;
        }

        public double? HotGlass { get; set; }
        public double? HotMat { get; set; }
        public double? MidGlass { get; set; }
        public double? ColdGlass { get; set; }
        public double? ColdMat { get; set; }
    }
}