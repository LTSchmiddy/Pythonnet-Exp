using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Python.Runtime;
using PythonEngine;

namespace BayatGames.SaveGamePro.Serialization.Types
{

	/// <summary>
	/// Save Game Type PyObject serialization implementation.
	/// </summary>
	public class SaveGameType_PyObject : SaveGameType
	{

		/// <summary>
		/// Gets the associated type for this custom type.
		/// </summary>
		/// <value>The type of the associated.</value>
		public override Type AssociatedType
		{
			get
			{
				return typeof ( Python.Runtime.PyObject );
			}
		}

		/// <summary>
		/// Write the specified value using the writer.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="writer">Writer.</param>
		public override void Write ( object value, ISaveGameWriter writer )
		{
			PyObject pyObject = ( PyObject )value;

			PickledPyObject pkl = new PickledPyObject(pyObject);

			writer.WriteProperty ("module", pkl.module);
			writer.WriteProperty ("data", pkl.data);
		}

		/// <summary>
		/// Read the data using the reader.
		/// </summary>
		/// <param name="reader">Reader.</param>
		public override object Read ( ISaveGameReader reader )
		{
			// return base.Read ( reader );
			
			return ReadData(reader).Unpickle();
		}

		/// <summary>
		/// Read the data into the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="reader">Reader.</param>
		public override void ReadInto ( object value, ISaveGameReader reader )
		{
			PyObject pyObject = ( PyObject )value;
			PickledPyObject pkl = ReadData(reader);
			pkl.UnpickleInto(pyObject);
			
		}

		private PickledPyObject ReadData (ISaveGameReader reader) {
			PickledPyObject pkl = new PickledPyObject();

			foreach ( string property in reader.Properties )			
			{
				switch ( property ) {
					case "module":
						pkl.module = reader.ReadProperty<string> ();
						break;
					case "data":
						pkl.data = reader.ReadProperty<byte[]> ();
						break;
				}
			}

			return pkl;
		}

	}

}