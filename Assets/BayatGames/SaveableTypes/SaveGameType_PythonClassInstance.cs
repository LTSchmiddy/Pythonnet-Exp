using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayatGames.SaveGamePro.Serialization.Types
{

	/// <summary>
	/// Save Game Type PythonClassInstance serialization implementation.
	/// </summary>
	public class SaveGameType_PythonClassInstance : SaveGameType
	{

		/// <summary>
		/// Gets the associated type for this custom type.
		/// </summary>
		/// <value>The type of the associated.</value>
		public override Type AssociatedType
		{
			get
			{
				return typeof ( PythonEngine.ScriptTypes.PythonClassInstance );
			}
		}

		/// <summary>
		/// Write the specified value using the writer.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="writer">Writer.</param>
		public override void Write ( object value, ISaveGameWriter writer )
		{
			PythonEngine.ScriptTypes.PythonClassInstance pythonClassInstance = ( PythonEngine.ScriptTypes.PythonClassInstance )value;
			writer.WriteProperty ( "moduleRef", pythonClassInstance.moduleRef.ModulePath );
			// writer.WriteProperty ( "moduleRef", pythonClassInstance.moduleRef );
			writer.WriteProperty ( "className", pythonClassInstance.className );
			writer.WriteProperty ( "instance", pythonClassInstance.instance );
		}

		/// <summary>
		/// Read the data using the reader.
		/// </summary>
		/// <param name="reader">Reader.</param>
		public override object Read ( ISaveGameReader reader )
		{
			PythonEngine.ScriptTypes.PythonClassInstance pythonClassInstance = new PythonEngine.ScriptTypes.PythonClassInstance ();
			ReadInto ( pythonClassInstance, reader );
			return pythonClassInstance;
		}

		/// <summary>
		/// Read the data into the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="reader">Reader.</param>
		public override void ReadInto ( object value, ISaveGameReader reader )
		{
			PythonEngine.ScriptTypes.PythonClassInstance pythonClassInstance = ( PythonEngine.ScriptTypes.PythonClassInstance )value;
			foreach ( string property in reader.Properties )
			{
				switch ( property )
				{
					case "moduleRef":
						pythonClassInstance.moduleRef.ModulePath = reader.ReadProperty<System.String> ();
						// pythonClassInstance.moduleRef = reader.ReadProperty<System.String> ();
						break;
					case "className":
						pythonClassInstance.className = reader.ReadProperty<System.String> ();
						break;
					case "instance":
						pythonClassInstance.instance = reader.ReadProperty<Python.Runtime.PyObject> ();
						break;
				}
			}
		}
		
	}

}