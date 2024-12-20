﻿using System.IO;
using UnityEngine;

namespace ObjectManagement.Scripts
{
	public class GameDataReader 
	{

		private readonly BinaryReader _reader;

		public GameDataReader (BinaryReader reader) 
		{
			_reader = reader;
		}

		public string ReadString() => _reader.ReadString();
		public float ReadFloat () => _reader.ReadSingle();

		public int ReadInt () => _reader.ReadInt32();

		public Color ReadColor () 
		{
			Color value;
			value.r = _reader.ReadSingle();
			value.g = _reader.ReadSingle();
			value.b = _reader.ReadSingle();
			value.a = _reader.ReadSingle();
			return value;
		}

		public Quaternion ReadQuaternion () 
		{
			Quaternion value;
			value.x = _reader.ReadSingle();
			value.y = _reader.ReadSingle();
			value.z = _reader.ReadSingle();
			value.w = _reader.ReadSingle();
			return value;
		}

		public Vector3 ReadVector3 () 
		{
			Vector3 value;
			value.x = _reader.ReadSingle();
			value.y = _reader.ReadSingle();
			value.z = _reader.ReadSingle();
			return value;
		}
	}
}