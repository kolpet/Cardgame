using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.AspectContainer
{
	public interface IContainer
	{
		T AddAspect<T>(string key = null) where T : IAspect, new();
		T AddAspect<T>(T aspect, string key = null) where T : IAspect;
		T GetAspect<T>(string key = null) where T : IAspect;
		ICollection<T> GetAspects<T>() where T : IAspect;
		ICollection<IAspect> Aspects();
	}

	public class Container : IContainer
	{
		Dictionary<string, IAspect> aspects = new Dictionary<string, IAspect>();

		public T AddAspect<T>(string key = null) where T : IAspect, new()
		{
			return AddAspect<T>(new T(), key);
		}

		public T AddAspect<T>(T aspect, string key = null) where T : IAspect
		{
			key = key ?? typeof(T).Name;
			aspects.Add(key, aspect);
			aspect.Container = this;
			return aspect;
		}

		public T GetAspect<T>(string key = null) where T : IAspect
		{
			key = key ?? typeof(T).Name;
			T aspect = aspects.ContainsKey(key) ? (T)aspects[key] : default(T);
			return aspect;
		}

		public ICollection<T> GetAspects<T>() where T : IAspect
		{
			return aspects.Values
				.Where(value => typeof(T) == value.GetType())
				.Select(value => (T)value).ToList();
		}

		public ICollection<IAspect> Aspects()
		{
			return aspects.Values;
		}
	}
}
