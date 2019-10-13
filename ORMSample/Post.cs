using System;
using System.Collections.Generic;
using System.Text;

namespace ORMSample
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public static implicit operator List<object>(Post v)
        {
            throw new NotImplementedException();
        }
    }
}
