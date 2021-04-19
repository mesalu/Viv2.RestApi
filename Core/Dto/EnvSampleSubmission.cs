using System;

namespace Viv2.API.Core.Dto
{
    /// <summary>
    /// DTO class for Core.Entities.EnvDataSample
    ///
    /// As per EnvDataSample, except the capture time is omitted (in favor of a centralized
    /// authority like the database assigning the capture time.) and the environment the sample
    /// was captured from is specified by ID, not nested object.
    ///
    /// Suitable for use as a form submission, or generic DTO representation of EnvDataSample
    /// </summary>
    public class EnvSampleSubmission
    {
        public Guid Environment { get; set; }
        public double? HotGlass { get; set; }
        public double? HotMat { get; set; }
        public double? MidGlass { get; set; }
        public double? ColdGlass { get; set; }
        public double? ColdMat { get; set; }
    }
}