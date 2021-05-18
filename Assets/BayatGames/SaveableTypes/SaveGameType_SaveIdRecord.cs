using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayatGames.SaveGamePro.Serialization.Types
{

	/// <summary>
	/// Save Game Type SaveIdRecord serialization implementation.
	/// </summary>
	public class SaveGameType_SaveIdRecord : SaveGameType
	{

		/// <summary>
		/// Gets the associated type for this custom type.
		/// </summary>
		/// <value>The type of the associated.</value>
		public override Type AssociatedType
		{
			get
			{
				return typeof ( LoadSave.SaveIdRecord );
			}
		}

		/// <summary>
		/// Write the specified value using the writer.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="writer">Writer.</param>
		public override void Write ( object value, ISaveGameWriter writer )
		{
			LoadSave.SaveIdRecord saveIdRecord = ( LoadSave.SaveIdRecord )value;
			writer.WriteProperty ( "info", saveIdRecord.info );
		}

		/// <summary>
		/// Read the data using the reader.
		/// </summary>
		/// <param name="reader">Reader.</param>
		public override object Read ( ISaveGameReader reader )
		{
			LoadSave.SaveIdRecord saveIdRecord = new LoadSave.SaveIdRecord ();
			ReadInto ( saveIdRecord, reader );
			return saveIdRecord;
		}

		/// <summary>
		/// Read the data into the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="reader">Reader.</param>
		public override void ReadInto ( object value, ISaveGameReader reader )
		{
			LoadSave.SaveIdRecord saveIdRecord = ( LoadSave.SaveIdRecord )value;
			foreach ( string property in reader.Properties )
			{
				switch ( property )
				{
					case "info":
						saveIdRecord.info = reader.ReadProperty<System.Collections.Generic.Dictionary<System.String, LoadSave.ISaveableInfo>> ();
						break;
				}
			}
		}
		
	}

}