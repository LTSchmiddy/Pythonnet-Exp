using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayatGames.SaveGamePro.Serialization.Types
{

	/// <summary>
	/// Save Game Type SavedPyObject serialization implementation.
	/// </summary>
	public class SaveGameType_SavedPyObject : SaveGameType
	{

		/// <summary>
		/// Gets the associated type for this custom type.
		/// </summary>
		/// <value>The type of the associated.</value>
		public override Type AssociatedType
		{
			get
			{
				return typeof ( PythonEngine.SavedPyBehaviourObject );
			}
		}

		/// <summary>
		/// Write the specified value using the writer.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="writer">Writer.</param>
		public override void Write ( object value, ISaveGameWriter writer )
		{
			PythonEngine.SavedPyBehaviourObject savedPyObject = ( PythonEngine.SavedPyBehaviourObject )value;
			writer.WriteProperty ( "pyObject", savedPyObject.pyObject );
		}

		/// <summary>
		/// Read the data using the reader.
		/// </summary>
		/// <param name="reader">Reader.</param>
		public override object Read ( ISaveGameReader reader )
		{
			PythonEngine.SavedPyBehaviourObject savedPyObject = new PythonEngine.SavedPyBehaviourObject ();
			ReadInto ( savedPyObject, reader );
			return savedPyObject;
		}

		/// <summary>
		/// Read the data into the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="reader">Reader.</param>
		public override void ReadInto ( object value, ISaveGameReader reader )
		{
			PythonEngine.SavedPyBehaviourObject savedPyObject = ( PythonEngine.SavedPyBehaviourObject )value;
			foreach ( string property in reader.Properties )
			{
				switch ( property )
				{
					case "pyObject":
						savedPyObject.pyObject = reader.ReadProperty<Python.Runtime.PyObject> ();
						break;
				}
			}
		}
		
	}

}