﻿using System.IO;
using UnityEngine;

namespace ObjectManagement.Scripts
{
	public class GameDataWriter {
		private readonly BinaryWriter _writer;

		public GameDataWriter (BinaryWriter writer) 
		{
			_writer = writer;
		}

		public void Write (float value) 
		{
			_writer.Write(value);
		}

		public void Write(string value) => _writer.Write(value);

		public void Write (int value) 
		{
			_writer.Write(value);
		}

		public void Write (Color value) 
		{
			_writer.Write(value.r);
			_writer.Write(value.g);
			_writer.Write(value.b);
			_writer.Write(value.a);
		}

		public void Write (Quaternion value) 
		{
			_writer.Write(value.x);
			_writer.Write(value.y);
			_writer.Write(value.z);
			_writer.Write(value.w);
		}

		public void Write (Vector3 value) 
		{
			_writer.Write(value.x);
			_writer.Write(value.y);
			_writer.Write(value.z);
		}
		
		public void Write (Vector4 color) 
		{
			_writer.Write(color.x);
			_writer.Write(color.y);
			_writer.Write(color.z);
			_writer.Write(color.w);
		}
	}
}