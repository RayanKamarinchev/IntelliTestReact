using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IntelliTest.Data.Enums;

namespace IntelliTest.Data.Entities
{
    public class Class
    {
        [Key]
        public int Id { get; set; }
        public Teacher Teacher { get; set; }
        [ForeignKey(nameof(Teacher))]
        public int TeacherId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public Subject Subject { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<ClassTest> ClassTests { get; set; }
        public List<StudentClass> Students { get; set; }
    }
}
