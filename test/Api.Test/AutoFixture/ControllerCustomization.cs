﻿using System;
using AutoFixture;
using AutoFixture.Kernel;
using Bit.Api.Controllers;
using Bit.Test.Common.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Org.BouncyCastle.Security;

namespace Bit.Api.Test.AutoFixture
{
    /// <summary>
    /// Disables setting of Auto Properties on the Controller to avoid ASP.net initialization errors. Still sets constructor dependencies.
    /// </summary>
    /// <param name="fixture"></param>
    public class ControllerCustomization : ICustomization
    {
        private readonly Type _controllerType;
        public ControllerCustomization(Type controllerType)
        {
            if (!controllerType.IsAssignableTo(typeof(Controller)))
            {
                throw new InvalidParameterException($"{nameof(controllerType)} must derive from {typeof(Controller).Name}");
            }

            _controllerType = controllerType;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new BuilderWithoutAutoProperties(_controllerType));
        }
    }
    public class ControllerCustomization<T> : ICustomization where T : Controller
    {
        public void Customize(IFixture fixture) => new ControllerCustomization(typeof(T)).Customize(fixture);
    }
}
