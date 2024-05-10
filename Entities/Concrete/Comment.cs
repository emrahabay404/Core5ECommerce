using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        
        public string CommContent { get; set; }
        public DateTime CommDate { get; set; }
        public bool Status { get; set; }



        [ForeignKey("AppUser")]
        public int Id { get; set; }
        public AppUser  AppUser{ get; set; }


        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }


        //[ForeignKey("Product")]
        //public int ProductId { get; set; }
        //public Product Product { get; set; }



    }
}
