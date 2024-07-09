using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;



public readonly struct KeyValueNode
{
	public KeyValueNode(string k)
	{
		key = k;
		value = string.Empty;
		values = new List<KeyValueNode>();
	}

	public KeyValueNode(string k, string v)
	{
		key = k;
		value = v;
		values = new List<KeyValueNode>();
	}

	public void Add(in KeyValueNode key)
	{
		values.Add(key);
	}


	public override string ToString()
	{
		return key;
	}

	public bool isEmpty =>
		key == KeyValueParser.NULL_NODE &&
		value == string.Empty &&
		values.Count == 0;

	public readonly string key;
	public readonly string value;
	public readonly List<KeyValueNode> values;
}

public sealed class KeyValueParser
{
	public const string NULL_NODE = "_null";


	public static KeyValueNode ParseFile(string path)
	{
		try
		{
			KeyValueParser kvp = new KeyValueParser(path);
			return kvp.m_Root;
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
			return new KeyValueNode(NULL_NODE);
		}
	}


	private enum TokenTypes
	{
		Value, BraceOpen, BraceClose, End
	}

	private readonly struct Token
	{
		readonly public TokenTypes type;
		readonly public string value;

		public Token(TokenTypes ttype, string tvalue)
		{
			type = ttype;
			value = tvalue;
		}
	}

	private static bool IsAlNum(char c)
	{
		if (c != '"' && c != '{' && c != ' ' && c != '\t' &&
			c != '\r' && c != '\n')
			return true;

		return false;
	}

	private Token LexNext()
	{
		bool readingValue = false;
		bool readingRawValue = false;
		bool readingComment = false;
		string value = "";

		while (m_lexPos < m_source.Length)
		{
			if (readingComment)
			{
				if (m_source[m_lexPos] == '\n')
					readingComment = false;

				m_lexPos++;
				continue;
			}

			if (readingValue || readingRawValue)
			{
				/*if (m_source[m_lexPos] != '"' &&
					m_source[m_lexPos] != '{' &&
					m_source[m_lexPos] != ' ' &&
					m_source[m_lexPos] != '\t' &&
					m_source[m_lexPos] != '\r' &&
					m_source[m_lexPos] != '\n')*/
				if ((readingRawValue && IsAlNum(m_source[m_lexPos])) || (readingValue && m_source[m_lexPos] != '"'))
				{
					value += m_source[m_lexPos];
					m_lexPos++;
					continue;
				}
				else
				{
					readingValue = false;
					m_lexPos++;
					return new Token(TokenTypes.Value, value);
				}
			}

			if (m_source[m_lexPos] == '"') // read value
			{
				readingValue = true;
				value = "";
				m_lexPos++;
			}
			else if (m_source[m_lexPos] == '{')
			{
				m_lexPos++;
				return new Token(TokenTypes.BraceOpen, "");
			}
			else if (m_source[m_lexPos] == '}')
			{
				m_lexPos++;
				return new Token(TokenTypes.BraceClose, "");
			}
			else if (m_source[m_lexPos] == '/')
			{
				if (readingValue)
				{
					value += m_source[m_lexPos];
					m_lexPos++;
				}
				else
				{
					readingComment = true;
					m_lexPos++;
				}
			}
			else if (m_source[m_lexPos] == ' ' ||
				m_source[m_lexPos] == '\t' ||
				m_source[m_lexPos] == '\n' ||
				m_source[m_lexPos] == '\r')
			{
				m_lexPos++;
			}
			else
			{
				if (!char.IsDigit(m_source[m_lexPos]))
				{
					readingRawValue = true;
					value = "";
				}
			}
		}

		return new Token(TokenTypes.End, "");
	}

	private void Parse()
	{
		m_lexPos = 0;
		m_Root = new KeyValueNode("_root");

		Token tok = LexNext();

		while (tok.type != TokenTypes.End)
		{
			if (tok.type == TokenTypes.Value)
			{
				LexNext(); // Eat {
				ParseBlock(tok, ref m_Root);
				tok = LexNext();
			}
			else
			{
				throw new Exception("Unexpected token. File is invalid");
			}
		}
	}

	private void ParseBlock(in Token name, ref KeyValueNode container)
	{
		Token token = LexNext();
		KeyValueNode values = new KeyValueNode(name.value);

		while (token.type != TokenTypes.BraceClose)
		{
			var key = token;
			var value = LexNext();

			if (value.type != TokenTypes.BraceOpen)
			{
				values.Add(new KeyValueNode(key.value, value.value));
			}
			else
			{
				ParseBlock(key, ref values);
			}

			token = LexNext();
		}

		container.Add(values);
	}

	private KeyValueParser(string fileName)
	{
		m_source = File.ReadAllText(fileName, Encoding.Default);
		Parse();
	}

	private KeyValueNode m_Root;
	private readonly string m_source;
	private int m_lexPos;
}
