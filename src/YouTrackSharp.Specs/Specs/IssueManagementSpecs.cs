using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using EasyHttp.Infrastructure;
using Machine.Specifications;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{
    [Subject(typeof(IssueManagement), "given authenticated connection and existing issues")]
    public class when_requesting_list_of_issues_for_a_project: AuthenticatedYouTrackConnectionForIssue
    {
     
        Because of = () =>
        {

            issues = issueManagement.GetIssues("SB", 10);
        };

        It should_return_list_of_issues_for_that_project = () => issues.ShouldNotBeNull();

        protected static IEnumerable<Issue> issues;
    }

    [Subject(typeof(IssueManagement), "given authenticated connection and existing issues") ]
    public class when_requesting_a_specific_issue: AuthenticatedYouTrackConnectionForIssue
    {

        Because of = () =>
        {
            issue = issueManagement.GetIssue("SB-282");

        };

        It should_return_issue_with_correct_id = () => issue.Id.ShouldEqual("SB-282");

        It should_return_issue_with_correct_project_name = () => issue.ProjectShortName.ShouldEqual("SB");


        static Issue issue;
    }

    [Subject(typeof(IssueManagement), "given authenticated connection and existing issues")]
    public class when_requesting_a_specific_issues_that_does_not_exist : AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () =>
        {
            exception = Catch.Exception(() => issueManagement.GetIssue("fdfdfsdfsd"));

        };

        It should_throw_invalid_request_exception = () => exception.ShouldBeOfType(typeof(InvalidRequestException));

        It should_contain_inner_exception_of_type_http_exception = () => exception.InnerException.ShouldBeOfType(typeof(HttpException));

        It inner_http_exception_should_contain_status_code_of_not_found = () => ((HttpException)exception.InnerException).StatusCode.ShouldEqual(HttpStatusCode.NotFound);

        static Exception exception;
    }

    [Subject(typeof(IssueManagement), "given authenticated connection and existing issues")]
    public class when_retrieving_comments_of_issue_that_has_comments: AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () =>
        {
            comments = issueManagement.GetCommentsForIssue("SB-560");

        };

        It should_return_the_comments = () => comments.ShouldNotBeNull();

        static IEnumerable<Comment> comments;
    }


    [Subject(typeof(IssueManagement), "given non-authenticated connection")]
    public class when_creating_a_new_issue_with_valid_information: YouTrackConnection
    {

        Establish context = () =>
        {
            IssueManagement = new IssueManagement(connection);

        };

        Because of = () =>
        {
            var issue = new Issue {ProjectShortName = "SB", Summary = "Issue Created"};


            exception = Catch.Exception(() => { IssueManagement.CreateIssue(issue); });
        };

        It should_throw_invalid_request_with_message_not_authenticated = () => exception.ShouldBeOfType(typeof(InvalidRequestException));

        It should_contain_message_not_logged_in = () => exception.Message.ShouldEqual("Not Logged In");

        protected static IssueManagement IssueManagement;
        static object response;
        static Exception exception;
    }

    [Subject(typeof(IssueManagement), "given authenticated connection")]
    public class when_creating_a_new_issue_with_valid_information_and_authenticated: AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () =>
        {

            var issue = new Issue
                        {
                            ProjectShortName = "SB",
                            Summary = "something new ",
                            Description = "somethingelse new too",
                            Assignee = "youtrackapi"
                        };

            id  = issueManagement.CreateIssue(issue);
        };

        It should_return_issue = () => id.ShouldNotBeEmpty();
        
        static string id;

    }
}