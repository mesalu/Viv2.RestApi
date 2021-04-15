using System;

namespace Viv2.API.Core.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public Species Species { get; set; }
        public string Name { get; set; }
        public string Morph { get; set; }
        public DateTime? HatchDate { get; set; }
    }
}