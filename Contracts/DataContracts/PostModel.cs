using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Contracts.DataContracts
{
    [Table("Post")]
    public class PostModel
    {
        private int m_ID;
        private string? m_Description;
        private IFormFile? m_Image;
        private byte[]? m_ImageData;

        public PostModel(PostModel posts)
        {
            this.ID = posts.ID;
            this.Description = posts.Description;
            this.Image = posts.Image;
            this.ImageData = posts.ImageData;
        }
        public PostModel()
        { 
        
        }

        [Key]
        public int ID { get => m_ID; set => m_ID = value; }
        [Required]
        public string? Description { get => m_Description; set => m_Description = value; }

        [JsonIgnore]
        [NotMapped]
        public IFormFile? Image { get => m_Image; set => m_Image = value; }

        public byte[]? ImageData { get => m_ImageData; set => m_ImageData = value; }

        [NotMapped]
        public List<CommentModel> PostComments { set; get; }
    }
}
