using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities {

    public class State : EntityBase<State>
    {
        public State() { }
        public virtual long Id { get; set; }
        [StringLength(100)]
        public virtual string Name { get; set; }

        /// <summary>
        /// just use in app
        /// </summary>
        public List<City> Cities { get; set; }
    }
}
