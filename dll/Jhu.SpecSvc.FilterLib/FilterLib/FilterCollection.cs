using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace Jhu.SpecSvc.FilterLib
{
	public class FilterCollection : CollectionBase, IEnumerable
	{

		public Filter this[int index]
		{
			get
			{
				return (Filter) List[index]; 
			}
			set
			{
				List[index] = value; 
			}
		}

		public void Add(Filter filter)
		{
			List.Add(filter);
		}

		public void Remove(int index)
		{
			List.RemoveAt(index); 
		}

		public void Remove(Filter filter)
		{
			List.Remove(filter);
		}

		public bool Contains(Filter filter)
		{
			return List.Contains(filter);
		}

		public int IndexOf(Filter filter)
		{
			return List.IndexOf(filter);
		}

		public void Insert(int index, Filter filter)
		{
			List.Insert(index, filter);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new FilterEnumerator(this);
		}

		public class FilterEnumerator : IEnumerator
		{
			// Member variables
			private int _index;				// current index
			FilterCollection _coll;		// references the enumerated collection

			// Constructs the object and initializes member variables
			// The enumerated collection is passed as a parameter
			public FilterEnumerator(FilterCollection coll)
			{
				_coll = coll;
				_index = -1;	// sets the current index before the first element
			}

			// Returns the element referenced by the _index member variable
			public Filter Current
			{
				get
				{
					return (Filter) _coll.List[_index];
				}
			}

			// Implements the IEnumerator interface
			object IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}

			// Resets the index reference before the first element
			public void Reset()
			{
				_index = -1;
			}

			// Advances the index reference variable and returns true if
			// there are remaining elements in the collection
			public bool MoveNext()
			{
				_index ++;
				return (_index < _coll.Count);
			}

		}
	}		//

}