# How to Contribute

We'd love to accept your patches, contributions and suggestions to this project.
Here are a few small guidelines you need to follow.

## Code of conduct

To better foster an open, innovative and inclusive community please refer to our
[Code of Conduct](CODE_OF_CONDUCT.md) when contributing.

### Report a bug

If you think you've found a bug, please log a new issue in the [GitHub issue
tracker. When filing issues, please use our [issue
template](.github/ISSUE_TEMPLATE.md). The best way to get your bug fixed is to
be as detailed as you can be about the problem. Providing a minimal project with
steps to reproduce the problem is ideal. Here are questions you can answer
before you file a bug to make sure you're not missing any important information.

1. Did you read the documentation?
2. Did you include the snippet of broken code in the issue?
3. What are the *EXACT* steps to reproduce this problem?
4. What specific version or build are you using?
5. What operating system are you using?

GitHub supports
[markdown](https://help.github.com/articles/github-flavored-markdown/), so when
filing bugs make sure you check the formatting before clicking submit.

### Make a suggestion

If you have an idea for a new feature or enhancement let us know by filing an
issue. To help us understand and prioritize your idea please provide as much
detail about your scenario and why the feature or enhancement would be useful.

## Contributing code and content

This is an open source project and we welcome code and content contributions
from the community.

**Identifying the scale**

If you would like to contribute to this project, first identify the scale of
what you would like to contribute. If it is small (grammar/spelling or a bug
fix) feel free to start working on a fix.

If you are submitting a feature or substantial code contribution, please discuss
it with the team. You might also read these two blogs posts on contributing
code: [Open Source Contribution
Etiquette](http://tirania.org/blog/archive/2010/Dec-31.html) by Miguel de Icaza
and [Don't "Push" Your Pull
Requests](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/) by
Ilya Grigorik. Note that all code submissions will be rigorously reviewed and
tested by the project team, and only those that meet an extremely high bar for
both quality and design/roadmap appropriateness will be merged into the source.

**Obtaining the source code**

If you are an outside contributor, please fork the repository to your account.
See the GitHub documentation for [forking a
repo](https://help.github.com/articles/fork-a-repo/) if you have any questions
about this. 

**Submitting a pull request**

If you don't know what a pull request is read this article:
https://help.github.com/articles/using-pull-requests. Make sure the repository
can build and all tests pass, as well as follow the current coding guidelines.
When submitting a pull request, please use our [pull request
template](.github/PULL_REQUEST_TEMPLATE.md).

Pull requests should all be done to the **master** branch.

---

## Code reviews

All submissions, including submissions by project members, require review. We
use GitHub pull requests for this purpose. Consult [GitHub
Help](https://help.github.com/articles/about-pull-requests/) for more
information on using pull requests.

## Releasing a new version of the NV.Templates NuGet Package

To release a new version of the NV.Templates NuGet Package, the developer needs to create a tag on a commit.
The publish workflow pipeline will automatically kick in.

To do that :
- Go to the [releases page](https://github.com/nventive/DotNet.Backend.Template/releases/new)
- In **Choose a tag**, type in the tag name, starting with a **v** (ex: v2.5.3), and **Create new tag: v2.5.3**
- Choose your target branch (typically master)
- Type in the **Release Title** (same as the tag name)
- A quick description of the release, if necessary
- Choose options (pre-release or latest release)
- Click **Publish release**

After, you can check the [actions](https://github.com/nventive/DotNet.Backend.Template/actions) to see that the publish pipeline is executing properly.

When the pipeline completes, it can take a few minutes for the package to be available on NuGet.

To make the package available locally, you will need to reinstall the packages with DotNetCLI.

## Community Guidelines

This project follows [Google's Open Source Community
Guidelines](https://opensource.google.com/conduct/).