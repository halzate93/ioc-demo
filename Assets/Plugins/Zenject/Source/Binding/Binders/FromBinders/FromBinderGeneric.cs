using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public class FromBinderGeneric<TContract> : FromBinder
    {
        public FromBinderGeneric(
            BindInfo bindInfo,
            BindFinalizerWrapper finalizerWrapper)
            : base(bindInfo, finalizerWrapper)
        {
            BindingUtil.AssertIsDerivedFromTypes(typeof(TContract), BindInfo.ContractTypes);
        }

        public ScopeArgConditionCopyNonLazyBinder FromFactory<TFactory>()
            where TFactory : IFactory<TContract>
        {
            return FromFactoryBase<TContract, TFactory>();
        }

        public ScopeArgConditionCopyNonLazyBinder FromMethod(Func<InjectContext, TContract> method)
        {
            return FromMethodBase<TContract>(method);
        }

        public ScopeArgConditionCopyNonLazyBinder FromMethodMultiple(Func<InjectContext, IEnumerable<TContract>> method)
        {
            return FromMethodMultipleBase<TContract>(method);
        }

        public ScopeConditionCopyNonLazyBinder FromResolveGetter<TObj>(Func<TObj, TContract> method)
        {
            return FromResolveGetter<TObj>(null, method);
        }

        public ScopeConditionCopyNonLazyBinder FromResolveGetter<TObj>(object identifier, Func<TObj, TContract> method)
        {
            return FromResolveGetterBase<TObj, TContract>(identifier, method);
        }

        public ScopeConditionCopyNonLazyBinder FromInstance(TContract instance)
        {
            return FromInstance(instance, false);
        }

        public ScopeConditionCopyNonLazyBinder FromInstance(TContract instance, bool allowNull)
        {
            return FromInstanceBase(instance, allowNull);
        }

#if !NOT_UNITY3D

        public ScopeArgConditionCopyNonLazyBinder FromComponentInChildren(bool excludeSelf = false)
        {
            BindingUtil.AssertIsInterfaceOrComponent(AllParentTypes);

            return FromMethodMultiple((ctx) =>
                {
                    Assert.That(ctx.ObjectType.DerivesFromOrEqual<MonoBehaviour>());
                    Assert.IsNotNull(ctx.ObjectInstance);

                    var res = ((MonoBehaviour)ctx.ObjectInstance).GetComponentsInChildren<TContract>()
                                                                 .Where(x => !ReferenceEquals(x, ctx.ObjectInstance));

                    if (excludeSelf) res = res.Where(x => (x as Component).gameObject != (ctx.ObjectInstance as Component).gameObject);

                    return res;
                });
        }

        public ScopeArgConditionCopyNonLazyBinder FromComponentInParents(bool excludeSelf = false)
        {
            BindingUtil.AssertIsInterfaceOrComponent(AllParentTypes);

            return FromMethodMultiple((ctx) =>
                {
                    Assert.That(ctx.ObjectType.DerivesFromOrEqual<MonoBehaviour>());
                    Assert.IsNotNull(ctx.ObjectInstance);

                    var res = ((MonoBehaviour)ctx.ObjectInstance).GetComponentsInParent<TContract>()
                        .Where(x => !ReferenceEquals(x, ctx.ObjectInstance));

                    if (excludeSelf) res = res.Where(x => (x as Component).gameObject != (ctx.ObjectInstance as Component).gameObject);

                    return res;
                });
        }

        public ScopeArgConditionCopyNonLazyBinder FromComponentSibling()
        {
            BindingUtil.AssertIsInterfaceOrComponent(AllParentTypes);

            return FromMethodMultiple((ctx) =>
                {
                    Assert.That(ctx.ObjectType.DerivesFromOrEqual<MonoBehaviour>());
                    Assert.IsNotNull(ctx.ObjectInstance);

                    return ((MonoBehaviour)ctx.ObjectInstance).GetComponents<TContract>()
                        .Where(x => !ReferenceEquals(x, ctx.ObjectInstance));
                });
        }

        public ScopeArgConditionCopyNonLazyBinder FromComponentInHierarchy()
        {
            BindingUtil.AssertIsInterfaceOrComponent(AllParentTypes);

            return FromMethodMultiple((ctx) =>
                {
                    return ctx.Container.Resolve<Context>().GetRootGameObjects()
                        .SelectMany(x => x.GetComponentsInChildren<TContract>())
                        .Where(x => !ReferenceEquals(x, ctx.ObjectInstance));
                });
        }
#endif
    }
}
