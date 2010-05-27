Mint gem
=====================

## Usage ##

    mint SevenDigital.A.dll

Produces a conflict report:

    ------------------------------------------
    Count of assemblies referenced
    ------------------------------------------
    SevenDigital.B_1.0.0.0                            1
    SevenDigital.C_1.0.0.0                            2
    SevenDigital.E_1.0.0.0                            3
    SevenDigital.D_1.0.0.0                            1
    ------------------------------------------
    Incorrect Dependencies Summary
    ------------------------------------------
    ***** Major Revision Conflict *****
    SevenDigital.A
    References:                     SevenDigital.B
    Reference Version:              1.0.0.0
    Actual Reference Version:       1.5.0.0

Mint rake tasks
=====================

##Installation##

Include the mint tasks in your rake file using something like this:

    require 'mint'

    DependencyReportTasks.new(:working_directory => 'path/to/some/bin')

Working directory is the directory containing the assembly you wish to analyse.

##Examples##

Use info to check configuration:

    rake dependency:info

For the example above, this outputs:

    Executable: C:/Ruby/lib/ruby/gems/1.8/gems/mint-0.0.2/bin/chubbyrain.exe
    Working directory: path/to/some/bin

Check dependencies:

    rake dependency:report['assembly_name.dll']

Produces a report like:

    Inspecting directory: C:/path/to/some/bin
    Analyzing dependencies of: Calrisian.API.Public.dll

    ------------------------------------------
    Count of assemblies referenced
    ------------------------------------------
    Calrisian.Web_1.4.6.342                        2
    Calrisian.Net_1.2                              2
