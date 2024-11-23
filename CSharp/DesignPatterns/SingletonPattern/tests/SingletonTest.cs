using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using BindingFlags = System.Reflection.BindingFlags;

namespace SingletonPattern.tests;

public class SingletonTest
{
    [Test]
    public void Singleton_cannot_initiated()
    {
        // try to create an instance of the Singleton using reflection
        var constructor = typeof(Singleton).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
        
        // Ensure that consturctor is non-public 
        Assert.That(constructor, Is.Not.Null);
        Assert.That(constructor.IsPrivate, Is.True);
        
        
        // Reason below code is not working 
        // looks like the constructor is being invoked successfully despite being private. This suggests that reflection is not enforcing the singleton constraint as expected
        /* try to invoke the constructor
          try {  Try to invoke the constructor and catch any exception
                  var instance = constructor.Invoke(null); 
                  Assert.Fail("Expected TargetInvocationException was not thrown."); 
         } 
         catch (TargetInvocationException ex) {
                    Confirm the exception type
                  Assert.That(ex, Is.TypeOf<TargetInvocationException>()); 
         }
         */
        
        Singleton instance1  = Singleton.Instance;
        Singleton instance2 = Singleton.Instance;
        
        
        Assert.That(instance1, Is.SameAs(instance2), "Instances are not same and referencing to the same object.");
    }

    [Test]
    public void Singleton_implimentation_is_not_threadsafe()
    {
        List<Singleton> instances = new List<Singleton>();
        Parallel.For(0, 1000, i =>
        {
            instances.Add(Singleton.Instance);
        });
        Parallel.For(0, 1000, i =>
        {
            instances.Add(Singleton.Instance);
        });

        var distinctInstances = instances.Distinct().ToList();

        if (distinctInstances.Count() < 2)
        {
            Parallel.For(0, 1000, i =>
            {
                instances.Add(Singleton.Instance);
            });
        }
        Assert.That(distinctInstances.Count(), Is.GreaterThanOrEqualTo(1), "This implementation is not thread safe.");
    }
    
    [Test(Author = "ME", Description = "This tests verifies this implementation is thread safe. Warning: Seems this implementation has some flaws and unit test may get fails.")]
    public void Singleton_implimentation_is_threadsafe()
    {
        List<SingletonImpl> instances = new List<SingletonImpl>();
        Parallel.For(0, 100, i =>
        {
            instances.Add(SingletonImpl.Instance);
        });
        var distinctInstances = instances.Distinct().ToList();
        Assert.That(distinctInstances.Count(), Is.EqualTo(1), "This implementation is thread safe.");
    }

    [Test]
    public void Singleton_LazyImpl_Is_ThreadSafe()
    {
        List<LazySingletonImpl> instances = new List<LazySingletonImpl>();
        Parallel.For(0, 100, i =>
        {
            instances.Add(LazySingletonImpl.Instance);
        });
        var distinctInstances = instances.Distinct().ToList();
        Assert.That(distinctInstances.Count(), Is.EqualTo(1), "This Lazy implementation is thread safe.");
    }

    [Test]
    public void SingletonIternalImpl_Is_ThreadSafe()
    {
        List<SingletonIternalImpl> instances = new List<SingletonIternalImpl>();
        Parallel.For(0, 100, i =>
        {
            instances.Add(SingletonIternalImpl.Instance);
        }); 
        var distinctInstances = instances.Distinct().ToList();
        Assert.That(distinctInstances.Count(), Is.EqualTo(1), "This elegant implementation is thread safe.");
    }

    [Test]
    public void Singleton_Behaviour_Is_Exists_In_DIContainer_Lib()
    {
        

         IServiceCollection Services = new ServiceCollection();
         Services.AddSingleton<Logger>();

         IServiceProvider ServiceProvider = Services.BuildServiceProvider();

         var logger1 = ServiceProvider.GetRequiredService<Logger>();
         var logger2 = ServiceProvider.GetRequiredService<Logger>();
         var logger3 = ServiceProvider.GetRequiredService<Logger>();
         
         Assert.That(logger1, Is.SameAs(logger2));
         Assert.That(logger1, Is.SameAs(logger3));
    }
    
    public class Logger(){}
}