using System;
using System.IO;
using System.Linq;
using Todo.Core;
using Xunit;

namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Add_IncreasesCount()
        {
            var list = new TodoList();
            list.Add(" task ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items.First().Title);
        }

        [Fact]
        public void Remove_ById_Works()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void Find_ReturnsMatches()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var found = list.Find("buy").ToList();
            Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }

        [Fact]
        public void SaveAndLoad_PersistsItems()
        {
            var list = new TodoList();
            var item1 = list.Add("Task 1");
            item1.MarkDone();
            list.Add("Task 2");

            var tempFile = Path.GetTempFileName();
            try
            {
                list.Save(tempFile);

                var loadedList = new TodoList();
                loadedList.Load(tempFile);

                Assert.Equal(2, loadedList.Count);
                Assert.Equal("Task 1", loadedList.Items[0].Title);
                Assert.True(loadedList.Items[0].IsDone);
                Assert.Equal("Task 2", loadedList.Items[1].Title);
                Assert.False(loadedList.Items[1].IsDone);
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
    }
}
