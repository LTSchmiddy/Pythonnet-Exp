using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayatGames.SaveGamePro.Serialization.Types
{

	/// <summary>
	/// Save Game Type SaveFile serialization implementation.
	/// </summary>
	public class SaveGameType_SaveFile : SaveGameType
	{

		/// <summary>
		/// Gets the associated type for this custom type.
		/// </summary>
		/// <value>The type of the associated.</value>
		public override Type AssociatedType
		{
			get
			{
				return typeof ( LoadSave.SaveFile );
			}
		}

		/// <summary>
		/// Write the specified value using the writer.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="writer">Writer.</param>
		public override void Write ( object value, ISaveGameWriter writer )
		{
			LoadSave.SaveFile saveFile = ( LoadSave.SaveFile )value;
			writer.WriteProperty ( "profileName", saveFile.profileName );
			writer.WriteProperty ( "sceneId", saveFile.sceneId );
			writer.WriteProperty ( "records", saveFile.records );
		}

		/// <summary>
		/// Read the data using the reader.
		/// </summary>
		/// <param name="reader">Reader.</param>
		public override object Read ( ISaveGameReader reader )
		{
			LoadSave.SaveFile saveFile = new LoadSave.SaveFile ();
			ReadInto ( saveFile, reader );
			return saveFile;
		}

		/// <summary>
		/// Read the data into the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="reader">Reader.</param>
		public override void ReadInto ( object value, ISaveGameReader reader )
		{
			LoadSave.SaveFile saveFile = ( LoadSave.SaveFile )value;
			foreach ( string property in reader.Properties )
			{
				switch ( property )
				{
					case "profileName":
						saveFile.profileName = reader.ReadProperty<System.String> ();
						break;
					case "sceneId":
						saveFile.sceneId = reader.ReadProperty<System.Guid> ();
						break;
					case "records":
						saveFile.records = reader.ReadProperty<System.Collections.Generic.Dictionary<System.Guid, LoadSave.SaveIdRecord>> ();
						break;
				}
			}
		}
		
	}

}