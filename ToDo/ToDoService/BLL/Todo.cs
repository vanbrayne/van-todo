using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoService.BLL
{
    public class Todo : BaseClass
    {
        protected ToDoService.Todo _DalTodo { get { return _DalObject as ToDoService.Todo; } }

        public string Name { get { return _DalTodo.Name; } }

        private Group _parent;
        public Group Parent
        {
            get
            {
                if ((null == _parent) && (null != _DalTodo.Fk_Group))
                {
                    _parent = Group.Get<Group>((Guid)_DalTodo.Fk_Group);
                    _parent.AddChild(this);
                }
                return _parent;
            }
        }

        public Todo()
        {
        }

        protected Todo(ToDoService.Todo dalTodo)
            : this()
        {
            _DalObject = dalTodo;
        }

        public void FillCache()
        {
            var ctx = GetContext();
            foreach (var dalTodo in ctx.Todoes.Where(x => 1 == 1))
            {
                // This will put the object in the cache
                var group = new Todo(dalTodo);
            }
        }
    }
}