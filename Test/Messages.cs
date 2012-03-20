﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ProtoBuf;

namespace Test
{
	[Serializable]
	[ProtoContract]
	[ProtoInclude(1, typeof(PrimitivesMessage))]
	[ProtoInclude(2, typeof(LongArraysMessage))]
	[ProtoInclude(3, typeof(S16Message))]
	[ProtoInclude(4, typeof(S32Message))]
	[ProtoInclude(5, typeof(U32Message))]
	[ProtoInclude(6, typeof(S64Message))]
	[ProtoInclude(7, typeof(ComplexMessage))]
	abstract class MessageBase
	{
		public abstract void Compare(MessageBase msg);

		protected static Random s_rand = new Random(123);

		public static void ResetSeed()
		{
			s_rand = new Random(123);
		}

		protected static void A(bool b)
		{
			if (!b)
				throw new Exception();
		}

		public static MessageBase[] CreateMessages(Type type, int numMessages)
		{
			var arr = new MessageBase[numMessages];

			for (int i = 0; i < numMessages; ++i)
				arr[i] = (MessageBase)Activator.CreateInstance(type, s_rand);

			return arr;
		}

		static byte[] r64buf = new byte[8];
		protected static long GetRandomInt64(Random random)
		{
			// XXX produces quite big numbers
			random.NextBytes(r64buf);
			return BitConverter.ToInt64(r64buf, 0);
		}
	}

	[Serializable]
	[ProtoContract]
	sealed class S16Message : MessageBase
	{
		[ProtoMember(1)]
		short m_val;

		public S16Message()
		{
		}

		public S16Message(Random r)
		{
			m_val = (short)r.Next();
		}

		public override void Compare(MessageBase msg)
		{
			var m = (S16Message)msg;
			A(m_val == m.m_val);
		}
	}

	[Serializable]
	[ProtoContract]
	sealed class S32Message : MessageBase
	{
		[ProtoMember(1)]
		int m_val;

		public S32Message()
		{
		}

		public S32Message(Random r)
		{
			m_val = (int)r.Next();
		}

		public override void Compare(MessageBase msg)
		{
			var m = (S32Message)msg;
			A(m_val == m.m_val);
		}
	}

	[Serializable]
	[ProtoContract]
	sealed class U32Message : MessageBase
	{
		[ProtoMember(1)]
		uint m_val;

		public U32Message()
		{
		}

		public U32Message(Random r)
		{
			m_val = (uint)r.Next();
		}

		public override void Compare(MessageBase msg)
		{
			var m = (U32Message)msg;
			A(m_val == m.m_val);
		}
	}

	[Serializable]
	[ProtoContract]
	sealed class S64Message : MessageBase
	{
		[ProtoMember(1)]
		long m_val;

		public S64Message()
		{
		}

		public S64Message(Random r)
		{
			m_val = GetRandomInt64(r);
		}

		public override void Compare(MessageBase msg)
		{
			var m = (S64Message)msg;
			A(m_val == m.m_val);
		}
	}

	[Serializable]
	[ProtoContract]
	sealed class PrimitivesMessage : MessageBase
	{
		[ProtoMember(1)]
		bool m_bool;

		[ProtoMember(2)]
		byte m_byte;
		[ProtoMember(3)]
		sbyte m_sbyte;
		[ProtoMember(4)]
		char m_char;
		[ProtoMember(5)]
		ushort m_ushort;
		[ProtoMember(6)]
		short m_short;
		[ProtoMember(7)]
		uint m_uint;
		[ProtoMember(8)]
		int m_int;
		[ProtoMember(9)]
		ulong m_ulong;
		[ProtoMember(10)]
		long m_long;

		[ProtoMember(11)]
		float m_single;
		[ProtoMember(12)]
		double m_double;

		public PrimitivesMessage()
		{
		}

		public PrimitivesMessage(Random r)
		{
			m_bool = (r.Next() & 1) == 1;
			m_byte = (byte)r.Next();
			m_sbyte = (sbyte)r.Next();
			m_char = (char)r.Next();
			m_ushort = (ushort)r.Next();
			m_short = (short)r.Next();
			m_uint = (uint)r.Next();
			m_int = (int)r.Next();
			m_ulong = (ulong)r.Next();
			m_long = (long)r.Next();

			m_int = r.Next();

			m_single = (float)r.NextDouble();
			m_double = r.NextDouble();
		}

		public override void Compare(MessageBase msg)
		{
			var m = (PrimitivesMessage)msg;

			A(m_bool == m.m_bool);

			A(m_byte == m.m_byte);
			A(m_sbyte == m.m_sbyte);
			A(m_char == m.m_char);
			A(m_ushort == m.m_ushort);
			A(m_short == m.m_short);
			A(m_uint == m.m_uint);
			A(m_int == m.m_int);
			A(m_ulong == m.m_ulong);
			A(m_long == m.m_long);

			A(m_single == m.m_single);
			A(m_double == m.m_double);
		}
	}



	[Serializable]
	[ProtoContract]
	sealed class LongArraysMessage : MessageBase
	{
		[ProtoMember(1)]
		byte[] m_byteArr;
		[ProtoMember(2)]
		int[] m_intArr;

		public LongArraysMessage()
		{
		}

		public LongArraysMessage(Random r)
		{
			if (r.Next(100) == 0)
			{
				m_byteArr = null;
			}
			else
			{
				m_byteArr = new byte[r.Next(10000, 100000)];
				r.NextBytes(m_byteArr);
			}

			if (r.Next(100) == 0)
			{
				m_intArr = null;
			}
			else
			{
				m_intArr = new int[r.Next(10000, 100000)];
				for (int i = 0; i < m_intArr.Length; ++i)
					m_intArr[i] = r.Next();
			}
		}

		public override void Compare(MessageBase msg)
		{
			var m = (LongArraysMessage)msg;

			if (m_byteArr == null)
			{
				A(m_byteArr == m.m_byteArr);
			}
			else
			{
				for (int i = 0; i < m_byteArr.Length; ++i)
					A(m_byteArr[i] == m.m_byteArr[i]);
			}

			if (m_intArr == null)
			{
				A(m_intArr == m.m_intArr);
			}
			else
			{
				for (int i = 0; i < m_intArr.Length; ++i)
					A(m_intArr[i] == m.m_intArr[i]);
			}
		}
	}



	[Serializable]
	[ProtoContract]
	sealed class SimpleSealedClass
	{
		[ProtoMember(1)]
		long m_val;

		public SimpleSealedClass()
		{
		}

		public SimpleSealedClass(Random r)
		{
			m_val = (long)r.Next();
		}

		public void Compare(SimpleSealedClass msg)
		{
			var m = (SimpleSealedClass)msg;
			if (m_val != m.m_val)
				throw new Exception();
		}
	}

	[Serializable]
	[ProtoContract]
	sealed class ComplexMessage : MessageBase
	{
		[ProtoMember(1)]
		string m_string;

		[ProtoMember(2)]
		S16Message m_msg;

		[ProtoMember(3)]
		int[] m_intArr;

		[ProtoMember(4)]
		SimpleSealedClass m_sealedClass;

		public ComplexMessage()
		{
		}

		public ComplexMessage(Random r)
		{
			if (r.Next(100) == 0)
				m_string = null;
			else
				m_string = new string((char)r.Next((int)'a', (int)'z'), r.Next(2, 100));

			if (r.Next(100) == 0)
				m_msg = null;
			else
				m_msg = new S16Message(r);

			if (r.Next(100) == 0)
				m_intArr = null;
			else
				m_intArr = new int[r.Next(1, 100)];

			if (r.Next(100) == 0)
				m_sealedClass = null;
			else
				m_sealedClass = new SimpleSealedClass(r);
		}

		public override void Compare(MessageBase msg)
		{
			var m = (ComplexMessage)msg;

			A(m_string == m.m_string);

			if (m_msg == null)
				A(m_msg == m.m_msg);
			else
				m_msg.Compare(m.m_msg);

			if (m_intArr == null)
			{
				A(m_intArr == m.m_intArr);
			}
			else
			{
				for (int i = 0; i < m_intArr.Length; ++i)
					A(m_intArr[i] == m.m_intArr[i]);
			}

			if (m_sealedClass == null)
				A(m_sealedClass == m.m_sealedClass);
			else
				m_sealedClass.Compare(m.m_sealedClass);
		}
	}
}
