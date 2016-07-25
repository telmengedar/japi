using System;
using System.Collections.Generic;
using System.Text;

namespace NightlyCode.Japi.Serialization.Data.Path {

    /// <summary>
    /// parses a path for use with node selection on java data elements
    /// </summary>
    public class DataPathParser {

        /// <summary>
        /// parses a path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IEnumerable<IPathElement> ParsePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                yield break;

            ParseMode mode = ParseMode.Name;
            StringBuilder literal = new StringBuilder();
            foreach(char c in path) {
                switch(c) {
                case '/':
                    if(mode == ParseMode.Index)
                        throw new InvalidOperationException("Unexpected path separator");
                    if(literal.Length > 0)
                        yield return new NodePath(literal.ToString());
                    mode = ParseMode.Name;
                    literal.Length = 0;
                    break;
                case '[':
                    if(literal.Length > 0) {
                        if(mode == ParseMode.Name) {
                            yield return new NodePath(literal.ToString());
                            literal.Length = 0;
                        }
                        else throw new InvalidOperationException("Unexpected index begin");
                    }
                    mode = ParseMode.Index;
                    break;
                case ']':
                    if(mode == ParseMode.Index) {
                        yield return new IndexPath(int.Parse(literal.ToString()));
                        literal.Length = 0;
                    }
                    else throw new InvalidOperationException("Unexpected index end");
                    mode = ParseMode.Name;
                    break;
                default:
                    literal.Append(c);
                    break;
                }
            }

            if(literal.Length <= 0)
                yield break;

            if (mode == ParseMode.Name)
                yield return new NodePath(literal.ToString());
            else throw new InvalidOperationException("Unexpected path end");
        }
    }
}