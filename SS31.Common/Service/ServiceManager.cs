﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace SS31.Common.Service
{
	public static class ServiceManager
	{
		private readonly static Dictionary<Type, IGameService> registeredServices;

		public static void UnregisterService<T>() where T : IGameService
		{
			Type type = typeof(T);
			if (!registeredServices.ContainsKey(type))
				return;
			T s = (T)registeredServices[type];

			registeredServices.Remove(type);
			s.Dispose();
		}
			
		public static void UnregisterAll()
		{
			Type loggerType = typeof(Logger);
			foreach(Type type in registeredServices.Keys)
			{
				if (type == loggerType)
					continue; // We want to remove the logger last, so the other services can use it while they are disposing.

				IGameService s = registeredServices[type];
				s.Dispose();
			}
			Logger logger = (Logger)registeredServices[loggerType];
			logger.Dispose();
			registeredServices.Clear();
		}

		public static bool HasService<T>() where T : IGameService
		{
			return registeredServices.ContainsKey(typeof(T));
		}
			
		public static T Resolve<T>() where T : IGameService
		{
			Type t = typeof(T);
			if (!registeredServices.ContainsKey(t))
			{
				ConstructorInfo cinfo = t.GetConstructors().FirstOrDefault();
				if (cinfo == null)
					throw new NoPublicConstructorException(t);

				ParameterInfo[] pinfo = cinfo.GetParameters();
				if (pinfo.Any())
					throw new InvalidConstructorParameterException("The service constructors cannot have any arguments.");

				IGameService inst = Activator.CreateInstance<T>();
				registeredServices.Add(t, inst);
			}

			return (T)registeredServices[t];
		}
			
		static ServiceManager()
		{
			registeredServices = new Dictionary<Type, IGameService>();
		}
	}
}