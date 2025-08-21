using System;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using Zerobject.Laboost.Runtime.Attributes;
using Zerobject.Laboost.Runtime.Builders;
using Zerobject.Laboost.Runtime.Core;
using Zerobject.Laboost.Runtime.Exceptions;
using Zerobject.Laboost.Runtime.Extensions.Internal;
using Zerobject.Laboost.Runtime.Factories;

namespace Zerobject.Laboost.Runtime.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Container"/> to simplify binding, resolution, injection and scope management.
    /// </summary>
    public static class ContainerExtensions
    {
        #region Binding / Contract Methods

        /// <summary>
        /// Starts a binding for the specified contract type.
        /// </summary>
        /// <typeparam name="TContract">The contract type to bind.</typeparam>
        /// <param name="container">The DI container to create the binding in.</param>
        /// <returns>
        /// Returns a <see cref="BindingBuilder"/> object which can be used to configure the binding.
        /// </returns>
        [PublicAPI]
        public static BindingBuilder Bind<TContract>(this Container container)
        {
            return Bind(container, typeof(TContract));
        }

        /// <summary>
        /// Starts a binding for the specified contract type (Type overload).
        /// </summary>
        /// <param name="container">The DI container to create the binding in.</param>
        /// <param name="contractType">The contract type to bind.</param>
        /// <returns>
        /// Returns a <see cref="BindingBuilder"/> object which can be used to configure the binding.
        /// </returns>
        [PublicAPI]
        public static BindingBuilder Bind(this Container container, Type contractType)
        {
            return new BindingBuilder(container, contractType);
        }

        /// <summary>
        /// Creates a binding for a specific instance of an object.
        /// </summary>
        /// <typeparam name="TContract">The contract type.</typeparam>
        /// <param name="container">The DI container.</param>
        /// <param name="instance">The concrete instance to bind.</param>
        /// <returns>The same binding builder for fluent chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if instance is null.</exception>
        /// <exception cref="BindingTypesMismatchException">Thrown if instance type is not assignable to the contract type.</exception>
        [PublicAPI]
        public static BindingBuilder BindInstance<TContract>(this Container container, TContract instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var contractType = typeof(TContract);
            var implType     = instance.GetType();

            Debug.Log(@$"Contract type: {contractType.Name}
Implementation type: {implType.Name}");

            if (!contractType.IsAssignableFrom(implType))
                throw new BindingTypesMismatchException(contractType, implType);

            return container.Bind<TContract>().To(implType).FromInstance(instance);
        }

        #endregion

        #region Implementation / Construction Methods

        /// <summary>
        /// Binds the contract to a concrete implementation type (generic version).
        /// </summary>
        /// <typeparam name="TImpl">The concrete implementation type.</typeparam>
        /// <param name="builder">The binding builder.</param>
        /// <returns>The same binding builder for fluent chaining.</returns>
        [PublicAPI]
        public static BindingBuilder To<TImpl>(this BindingBuilder builder)
        {
            return To(builder, typeof(TImpl));
        }

        /// <summary>
        /// Binds the contract to a concrete implementation type.
        /// </summary>
        /// <param name="builder">The binding builder.</param>
        /// <param name="implType">The concrete implementation type.</param>
        /// <returns>The same binding builder for fluent chaining.</returns>
        /// <exception cref="BindingTypesMismatchException">Thrown if the implementation type is not assignable to the contract type.</exception>
        [PublicAPI]
        public static BindingBuilder To(this BindingBuilder builder, Type implType)
        {
            if (!builder.ContractType.IsAssignableFrom(implType))
                throw new BindingTypesMismatchException(builder.ContractType, implType);

            builder.ImplType = implType;
            builder.UpdateBinding();
            return builder;
        }

        /// <summary>
        /// Uses a constructor-based factory to create new instances of the bound type.
        /// </summary>
        /// <param name="builder">The binding builder.</param>
        /// <returns>The same binding builder for fluent chaining.</returns>
        [PublicAPI]
        public static BindingBuilder FromNew(this BindingBuilder builder)
        {
            var genFactoryType = typeof(CtorFactory<>).MakeGenericType(builder.ImplType);
            builder.Factory = Activator.CreateInstance(genFactoryType, builder.Container);
            builder.UpdateBinding();
            return builder;
        }

        /// <summary>
        /// Uses a factory method to create instances of the bound type.
        /// </summary>
        /// <typeparam name="TImpl">Implementation type returned by the factory method.</typeparam>
        /// <param name="builder">The binding builder.</param>
        /// <param name="method">The factory method.</param>
        /// <returns>The same binding builder.</returns>
        /// <exception cref="BindingNotConfiguredException">If ImplType is not set before calling this method.</exception>
        /// <exception cref="FactoryTypeMismatchException">If the factory method return type does not match ImplType.</exception>
        [PublicAPI]
        public static BindingBuilder FromMethod<TImpl>(this BindingBuilder builder, FactoryMethod<TImpl> method)
        {
            if (builder.ImplType == null)
                throw new BindingNotConfiguredException(builder.ContractType);

            if (!builder.ImplType.IsAssignableFrom(typeof(TImpl)))
                throw new FactoryTypeMismatchException(typeof(TImpl), builder.ImplType);

            builder.Factory = Activator.CreateInstance(typeof(MethodFactory<TImpl>), builder.Container, method);
            builder.UpdateBinding();
            return builder;
        }

        /// <summary>
        /// Binds a specific instance to the contract.
        /// </summary>
        /// <param name="builder">The binding builder.</param>
        /// <param name="instance">The concrete instance to bind.</param>
        /// <returns>The same binding builder for fluent chaining.</returns>
        /// <exception cref="ArgumentNullException">If the instance is null.</exception>
        /// <exception cref="BindingTypesConflictException">If ImplType is already set to a different type.</exception>
        [PublicAPI]
        public static BindingBuilder FromInstance(this BindingBuilder builder, object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var instanceType = instance.GetType();

            if (builder.ImplType != null && builder.ImplType != instanceType)
                throw new BindingTypesConflictException(builder.ImplType, instanceType);

            var genFactoryType = typeof(InstanceFactory<>).MakeGenericType(instanceType);
            builder.Factory  =   Activator.CreateInstance(genFactoryType, builder.Container, instance);
            builder.ImplType ??= instanceType;

            builder.UpdateBinding();
            return builder;
        }

        #endregion

        #region Identity / Metadata

        /// <summary>
        /// Sets an identifier for the binding, allowing differentiation between multiple bindings of the same contract.
        /// </summary>
        /// <param name="builder">The binding builder.</param>
        /// <param name="id">The identifier string.</param>
        /// <returns>The same binding builder for fluent chaining.</returns>
        [PublicAPI]
        public static BindingBuilder WithId(this BindingBuilder builder, string id)
        {
            builder.Id = id ?? throw new ArgumentNullException(nameof(id));
            builder.UpdateBinding();
            return builder;
        }

        #endregion

        #region Scope / Lifetime

        /// <summary>
        /// Marks the binding as a singleton across the container hierarchy.
        /// </summary>
        [PublicAPI]
        public static void AsSingle(this BindingBuilder builder)
        {
            OverrideBindingScope(builder, Scope.Single);
        }

        /// <summary>
        /// Marks the binding as cached within the container.
        /// </summary>
        [PublicAPI]
        public static void AsCached(this BindingBuilder builder)
        {
            OverrideBindingScope(builder, Scope.Cached);
        }

        /// <summary>
        /// Marks the binding as transient (new instance every resolution).
        /// </summary>
        [PublicAPI]
        public static void AsTransient(this BindingBuilder builder)
        {
            OverrideBindingScope(builder, Scope.Transient);
        }

        private static void OverrideBindingScope(BindingBuilder builder, Scope scope)
        {
            builder.Scope = scope;
            builder.UpdateBinding();
            builder.FinalizeBinding();
        }

        #endregion

        #region Resolve / Inject Helpers

        /// <summary>
        /// Resolves a contract type from the container.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="container">The DI container.</param>
        /// <param name="id">Optional binding identifier.</param>
        /// <returns>An instance of the requested type.</returns>
        [PublicAPI]
        public static T Resolve<T>(this Container container, string id = null)
        {
            return (T)Resolve(container, typeof(T), id);
        }

        /// <summary>
        /// Resolves a contract type from the container.
        /// </summary>
        /// <param name="container">The DI container.</param>
        /// <param name="type">The type to resolve.</param>
        /// <param name="id">Optional binding identifier.</param>
        /// <returns>An instance of the requested type.</returns>
        [PublicAPI]
        public static object Resolve(this Container container, Type type, string id = null)
        {
            BindingKey key              = new(type, id);
            var        bindingContainer = container;
            Binding    binding          = default;

            while (bindingContainer != null)
            {
                if (bindingContainer.Bindings.TryGetValue(key, out binding))
                    break;

                bindingContainer = bindingContainer.Parent;
            }

            if (bindingContainer == null)
                throw new BindingNotFoundException(type);

            switch (binding.Scope)
            {
                case Scope.Transient:
                    return binding.Factory.Create();

                case Scope.Cached:
                {
                    if (container.CachedInstances.TryGetValue(key, out var cached))
                        return cached;

                    cached                         = binding.Factory.Create();
                    container.CachedInstances[key] = cached;

                    return cached;
                }

                case Scope.Single:
                {
                    if (bindingContainer.SingleInstances.TryGetValue(key, out var instance))
                        return instance;

                    instance = binding.Factory.Create();

                    var c = bindingContainer;
                    while (c != null)
                    {
                        c.SingleInstances[key] = instance;
                        c                      = c.Parent;
                    }

                    return instance;
                }
                default:
                    throw new InvalidOperationException($"Неизвестный Scope: '{binding.Scope}'.");
            }
        }

        /// <summary>
        /// Injects dependencies into the target object's fields, properties, and methods.
        /// </summary>
        /// <param name="container">The DI container.</param>
        /// <param name="target">The target object to inject dependencies into.</param>
        /// <exception cref="ArgumentNullException">If the target is null.</exception>
        [PublicAPI]
        public static void Inject(this Container container, object target)
        {
            const BindingFlags bindingFlags
                = BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic;

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var targetType = target.GetType();
            var fields     = targetType.GetFields(bindingFlags);
            var props      = targetType.GetProperties(bindingFlags);
            var methods    = targetType.GetMethods(bindingFlags);

            foreach (var field in fields)
            {
                if (!field.TryGetAttribute(out InjectAttribute injectAttr))
                    continue;

                try
                {
                    field.SetValue(target, Resolve(container, field.FieldType, injectAttr.Id));
                }
                catch (Exception)
                {
                    if (!injectAttr.Optional) throw;
                }
            }

            foreach (var prop in props)
            {
                if (!prop.TryGetAttribute(out InjectAttribute injectAttr) || !prop.CanWrite)
                    continue;

                try
                {
                    var value = Resolve(container, prop.PropertyType, injectAttr.Id);
                    prop.SetValue(target, value);
                }
                catch (Exception)
                {
                    if (!injectAttr.Optional) throw;
                }
            }

            foreach (var method in methods)
            {
                if (!method.TryGetAttribute(out InjectAttribute injectAttr)
                 || method.IsStatic || method.IsAbstract)
                    continue;

                var @params = method.GetParameters();
                var args    = new object[@params.Length];
                var skip    = false;

                for (var i = 0; i < @params.Length; i++)
                {
                    var param           = @params[i];
                    var paramInjectAttr = param.TryGetAttribute(out InjectAttribute attr) ? attr : injectAttr;

                    try
                    {
                        args[i] = Resolve(container, param.ParameterType, paramInjectAttr.Id);
                    }
                    catch (Exception ex)
                    {
                        if (!paramInjectAttr.Optional)
                            Debug.Log(ex.Message);
                        skip = true;
                        break;
                    }
                }

                if (!skip)
                    method.Invoke(target, args);
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Checks if the container has a binding for the specified contract type.
        /// </summary>
        /// <typeparam name="TContract">The contract type.</typeparam>
        /// <param name="container">The DI container.</param>
        /// <param name="id">Optional binding identifier.</param>
        /// <returns>True if the binding exists; otherwise false.</returns>
        [PublicAPI]
        public static bool HasBinding<TContract>(this Container container, string id = null)
        {
            return HasBinding(container, typeof(TContract), id);
        }

        /// <summary>
        /// Checks if the container has a binding for the specified contract type.
        /// </summary>
        /// <param name="container">The DI container.</param>
        /// <param name="contractType">The contract type.</param>
        /// <param name="id">Optional binding identifier.</param>
        /// <returns>True if the binding exists; otherwise false.</returns>
        [PublicAPI]
        public static bool HasBinding(this Container container, Type contractType, string id = null)
        {
            return container.Bindings.ContainsKey((contractType, id));
        }

        #endregion
    }
}