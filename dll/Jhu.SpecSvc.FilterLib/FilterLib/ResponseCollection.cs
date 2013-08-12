using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace Jhu.SpecSvc.FilterLib
{


	public class ResponseCollection : ArrayList, IEnumerable
	{
		internal Filter _parent;

		public ResponseCollection()
		{
			_parent = null;
		}

		public ResponseCollection(Filter parent)
		{
			_parent = parent;
		}

		public new Response this[int index]
		{
			get
			{
				return (Response) base[index]; 
			}
			set
			{
				base[index] = value; 
			}
		}

		public void Add(Response response)
		{
			base.Add(response);
		}

		public Response Add(double wavelength, double responseValue)
		{
			Response response = new Response(_parent.DatabaseConnection, _parent.DatabaseTransaction);
			response.Wavelength = wavelength;
			response.Value = responseValue;

			base.Add(response);

			return response;
		}

		public void Remove(int index)
		{
			base.RemoveAt(index); 
		}

		public void Remove(Response response)
		{
			base.Remove(response);
		}

		public bool Contains(Response response)
		{
			return base.Contains(response);
		}

		public int IndexOf(Response response)
		{
			return base.IndexOf(response);
		}

		public void Insert(int index, Response response)
		{
			base.Insert(index, response);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ResponseEnumerator(this);
		}

		public class ResponseEnumerator : IEnumerator
		{
			// Member variables
			private int _index;				// current index
			ResponseCollection _coll;		// references the enumerated collection

			// Constructs the object and initializes member variables
			// The enumerated collection is passed as a parameter
			public ResponseEnumerator(ResponseCollection coll)
			{
				_coll = coll;
				_index = -1;	// sets the current index before the first element
			}

			// Returns the element referenced by the _index member variable
			public Response Current
			{
				get
				{
					return (Response) _coll[_index];
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
