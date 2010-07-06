namespace SharpArch.Specifications
{
    using Machine.Specifications;
    using Machine.Specifications.AutoMocking.Rhino;

    public abstract class specification_for_category_tasks : Specification<string, string>
    {
    }

    [Subject("")]
    public class when_the_category_tasks_are_asked_to_get_all : specification_for_category_tasks
    {

        Establish context = () =>
        {
     
        };

        Because of;

        It should_ask_the_category_repository_to_get_all;
    }
}