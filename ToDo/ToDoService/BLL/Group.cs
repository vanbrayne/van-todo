using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoService.BLL
{
    public class Group : BaseClass
    {
        protected ToDoService.Group _DalGroup { get { return _DalObject as ToDoService.Group; } }

        public string Name { get { return _DalGroup.Name; } }

        public List<Todo> TodoChildren { get; private set; }
        public List<Group> GroupChildren { get; private set; }

        private Group _parent;
        public Group Parent
        {
            get
            {
                if ((null == _parent) && (null != _DalGroup.Fk_Parent_Group))
                {
                    _parent = Group.Get<Group>((Guid)_DalGroup.Fk_Parent_Group);
                    _parent.AddChild(this);
                }
                return _parent;
            }
        }

        public Group()
        {
            TodoChildren = new List<Todo>();
            GroupChildren = new List<Group>();
        }

        protected Group(ToDoService.Group dalGroup)
            :this()
        {
            _DalObject = dalGroup;
        }

        internal void AddChild(Todo todo)
        {
            TodoChildren.Add(todo);
        }

        internal void AddChild(Group group)
        {
            GroupChildren.Add(group);
        }

        public void FillCache()
        {
            var ctx = GetContext();
            foreach (var dalGroup in ctx.Groups.Where(x => 1 == 1))
            {
                // This will put the object in the cache
                var group = new Group(dalGroup);
            }
        }
    }
}