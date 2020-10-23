using Xunit;
using LibGit2Sharp;
using Shouldly;
using System.Linq;

namespace Versionize.Tests
{
    public class ChangelogLinkBuilderFactoryTests
    {
        [Fact]
        public void ShouldCreateAGithubUrlBuilderForHTTPSPushUrls()
        {
            var repo = SetupRepositoryWithRemote("origin", "https://github.com/saintedlama/versionize.git");
            var linkBuilder = ChangelogLinkBuilderFactory.CreateFor(repo);

            linkBuilder.ShouldBeAssignableTo<GithubLinkBuilder>();
        }

        [Fact]
        public void ShouldCreateAGithubUrlBuilderForSSHPushUrls()
        {
            var repo = SetupRepositoryWithRemote("origin", "git@github.com:saintedlama/versionize.git");
            var linkBuilder = ChangelogLinkBuilderFactory.CreateFor(repo);

            linkBuilder.ShouldBeAssignableTo<GithubLinkBuilder>();
        }

        [Fact]
        public void ShouldPickFirstRemoteInCaseNoOriginWasFound()
        {
            var repo = SetupRepositoryWithRemote("some", "git@github.com:saintedlama/versionize.git");
            var linkBuilder = ChangelogLinkBuilderFactory.CreateFor(repo);

            linkBuilder.ShouldBeAssignableTo<GithubLinkBuilder>();
        }

        [Fact]
        public void ShouldFallbackToNoopInCaseNoGithubPushUrlWasDefined()
        {
            var repo = SetupRepositoryWithRemote("origin", "https://hostmeister.com/saintedlama/versionize.git");
            var linkBuilder = ChangelogLinkBuilderFactory.CreateFor(repo);

            linkBuilder.ShouldBeAssignableTo<PlainLinkBuilder>();
        }

        private Repository SetupRepositoryWithRemote(string remoteName, string pushUrl) 
        {
            var repo = new Repository();
            
            foreach (var existingRemoteName in repo.Network.Remotes.Select(remote => remote.Name)) {
              repo.Network.Remotes.Remove(existingRemoteName);
            }

            repo.Network.Remotes.Add(remoteName, pushUrl);
            
            return repo;
        }
    }
}