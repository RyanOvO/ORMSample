using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ORMSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var flag = PracticeAsync().GetAwaiter().GetResult();
            Console.WriteLine($"{(flag ? "测试成功" : "测试失败")}");
            Console.Read();
        }

        static async Task<bool> PracticeAsync()
        {
            using (var context = new BloggingContext())
            {
                bool flag = false;
                Console.WriteLine("------------- 正常插入数据 --------------");
                var blogs = new Blog
                {
                    Url = $"http://blogs.msdn.com/adonet/{Guid.NewGuid()}",
                    Posts = new List<Post>
                    {
                        new Post { Title = $"Intro to C# {Guid.NewGuid().ToString()}", Content = $"C#文章内容更新 --- {Guid.NewGuid()}" },
                        new Post { Title = $"Intro to VB.NET {Guid.NewGuid().ToString()}", Content = $"VB.NET文章内容更新 " +
                        $"---{Guid.NewGuid()}" },
                        new Post { Title = $"Intro to F# {Guid.NewGuid().ToString()}", Content = $"F#文章内容更新 " +
                        $"--- {Guid.NewGuid()}" }
                    }
                };
                context.Blogs.Add(blogs);

                if (await context.SaveChangesAsync(CancellationToken.None) > 0)
                    flag = true;

                Console.WriteLine("------------- 插入成功，Shadow测试成功  -----------------");

                Console.WriteLine("\r\n-------------- 更新数据 -------------");
                var myblog = context.Blogs.OrderBy(o => o.BlogId).First();
                myblog.Url = $"http://https://www.cnblogs.com/zhiyong-ITNote/{Guid.NewGuid()}";
                myblog.Posts = new List<Post>
                {
                    new Post
                    {
                        Title = $"更新shadow测试 {Guid.NewGuid()} 11111 ",
                        Content = $"更新模式下，Blog表CreatedTime不会插入数据，但是Post表的CreatedTime数据将会更新 {Guid.NewGuid()}"
                    },
                    new Post
                    {
                        Title = $"更新shadow测试 {Guid.NewGuid()} 2222 ",
                        Content = $"更新模式下，Blog表CreatedTime不会插入数据，但是Post表的CreatedTime数据将会更新 {Guid.NewGuid()}"
                    }
                };

                if (await context.SaveChangesAsync(CancellationToken.None) < 0)
                    flag = false;

                Console.WriteLine("------------- 更新成功，Shadow测试成功  -----------------");
                return flag;
            }
        }
    }
}
