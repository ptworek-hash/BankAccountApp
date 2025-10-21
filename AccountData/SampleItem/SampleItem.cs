using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Model.AccountData
{
	internal class SampleItem:ISampleItem 
	{
		private SampleItemDto _record;

		public SampleItem(SampleItemDto record = null) {
			if(record==null){ record = new SampleItemDto(); }
			_record = record;
		}

		public long Id { 
			get { return _record.Id; }
			set { _record.Id = value; } 
		}
		public string Nm {
			get { return _record.Nm; }
			set { _record.Nm = value; }
		}

		public bool Delete() {
			throw new NotImplementedException();
		}

		public bool Save() {
			throw new NotImplementedException();
		}
	}
}
