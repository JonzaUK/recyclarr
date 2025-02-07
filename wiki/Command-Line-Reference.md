Command line interface documentation for the `recyclarr` executable.

## Subcommands

Each service (Sonarr, Radarr) has a subcommand that must be specified in order to perform operations
related to that service, such as parsing relevant TRaSH guides and invoking API endpoints to modify
settings on that instance. As always, the `--help` option may be specified following a subcommand to
see more information directly in your terminal.

- `sonarr`: Update release profiles and quality definitions on configured Sonarr instances.
- `radarr`: Update custom formats and quality definitions on configured Radarr instances.

## Common Arguments

These are optional arguments shared by *all* subcommands.

### `--config`

One or more paths to YAML configuration files. Only the relevant configuration section for the
specified subcommand will be read from each file. If this argument is not specified, a single
default configuration file named `recyclarr.yml` will be used. It must be in the [[application data
directory|File-Structure]].

**Command Line Examples**:

```bash
# Default Config (recyclarr.yml)
recyclarr sonarr

# Single Config
recyclarr sonarr --config ../myconfig.yml

# Multiple Config
recyclarr sonarr --config ../myconfig1.yml "files/my config 2.yml"
```

### `--preview`

Performs a "dry run" by parsing the guide and printing the parsed data in a readable format to the
user. This does *not* perform any API calls to Radarr or Sonarr. You may want to run a preview if
you'd like to see if the guide is parsed correctly before updating your instance.

Example output for Sonarr Release Profile parsing

```txt
First Release Profile
  Include Preferred when Renaming?
    CHECKED

  Must Not Contain:
    /(\[EMBER\]|-EMBER\b|DaddySubs)/i

  Preferred:
    100        /\b(amzn|amazon)\b(?=[ ._-]web[ ._-]?(dl|rip)\b)/i
    90         /\b(dsnp|dsny|disney)\b(?=[ ._-]web[ ._-]?(dl|rip)\b)/i

Second Release Profile
  Include Preferred when Renaming?
    NOT CHECKED

  Preferred:
    180        /(-deflate|-inflate)\b/i
    150        /(-AJP69|-BTN|-CasStudio|-CtrlHD|-KiNGS)\b/i
    150        /(-monkee|-NTb|-NTG|-QOQ|-RTN)\b/i
```

Example output for Sonarr Quality Definition parsing

```txt
Quality              Min        Max
-------              ---        ---
HDTV-720p            2.3        67.5
HDTV-1080p           2.3        137.3
WEBRip-720p          4.3        137.3
WEBDL-720p           4.3        137.3
Bluray-720p          4.3        137.3
WEBRip-1080p         4.5        257.4
WEBDL-1080p          4.3        253.6
Bluray-1080p         4.3        258.1
Bluray-1080p Remux   0          400
HDTV-2160p           69.1       350
WEBRip-2160p         69.1       350
WEBDL-2160p          69.1       350
Bluray-2160p         94.6       400
Bluray-2160p Remux   204.4      400
```

### `--debug`

By default, Info, Warning and Error log levels are displayed in the console. This option enables
Debug level logs to be displayed. This is designed for debugging and development purposes and
generally will be too noisy for normal program usage.

### `--app-data`

Overrides the normal, default location of the [[application data directory|File-Structure]]. Note
that this option is mainly intended for usage in the official Docker image. It is not intended for
normal use outside of that.

## Sonarr

### `--list-release-profiles`

Prints a list of all [available Sonarr Release Profiles][sonarrjson] from the TRaSH Guides in YAML
format, ready to be copied & pasted directly into your `trash.yml` file. Here is an example of the
output you will see:

```txt
./trash sonarr --list-release-profiles

List of Release Profiles in the TRaSH Guides:

          - EBC725268D687D588A20CBC5F97E538B # Low Quality Groups
          - 76e060895c5b8a765c310933da0a5357 # Optionals
          - 71899E6C303A07AF0E4746EFF9873532 # P2P Groups + Repack/Proper
          - 1B018E0C53EC825085DD911102E2CA36 # Release Sources (Streaming Service)
          - d428eda85af1df8904b4bbe4fc2f537c # Anime - First release profile
          - 6cd9e10bb5bb4c63d2d7cd3279924c7b # Anime - Second release profile

The above Release Profiles are in YAML format and ready to be copied & pasted under the `trash_ids:` property.
```

You can copy & paste these directly into your `trash.yml` like this:

```yml
sonarr:
  - base_url: http://127.0.0.1:8989/sonarr
    api_key: 2424b3643507485ea2e06382d3f0b8a3
    release_profiles:
      - trash_ids:
          - d428eda85af1df8904b4bbe4fc2f537c # Anime - First release profile
          - 6cd9e10bb5bb4c63d2d7cd3279924c7b # Anime - Second release profile
```

### `--list-terms`

Prints a list of all terms (that have been assigned their own Trash IDs) for the Release Profile
with the specified Trash ID. Use the `--list-release-profiles` option to first get a list of the
[available Sonarr Release Profiles][sonarrjson] from the TRaSH Guides. Copy one of the Trash ID
values from there and provide it as the argument to this command to get its list of terms. The terms
are printed in YAML format, ready to be copied & pasted directly into your `trash.yml` file. Here is
an example of the output you will see:

```txt
./trash sonarr --list-terms 76e060895c5b8a765c310933da0a5357

List of Terms for the 'Optionals' Release Profile that may be filtered:

Ignored Terms:

            - cec8880b847dd5d31d29167ee0112b57 # Golden rule
            - 436f5a7d08fbf02ba25cb5e5dfe98e55 # Ignore Dolby Vision without HDR10 fallback.
            - f3f0f3691c6a1988d4a02963e69d11f2 # Ignore The Group -SCENE
            - 5bc23c3a055a1a5d8bbe4fb49d80e0cb # Ignore so called scene releases

Preferred Terms:

            - ea83f4740cec4df8112f3d6dd7c82751 # Prefer Season Packs
            - bc7a6383cbe88c3ee2d6396e1aacc0b3 # Prefer HDR
            - fa47da3377076d82d07c4e95b3f13d07 # Prefer Dolby Vision
            - 6f2aefa61342a63387f2a90489e90790 # Dislike retags: rartv, rarbg, eztv, TGx
            - 19cd5ecc0a24bf493a75e80a51974cdd # Dislike retagged groups
            - 6a7b462c6caee4a991a9d8aa38ce2405 # Dislike release ending: en
            - 236a3626a07cacf5692c73cc947bc280 # Dislike release containing: 1-

The above Term Filters are in YAML format and ready to be copied & pasted under the `include:` or `exclude:` filter properties.
```

You can copy & paste the lists above directly into your YAML under the desired `include:` or
`exclude:` filter sections. See the example below.

```yml
sonarr:
  - base_url: http://127.0.0.1:8989/sonarr
    api_key: 2424b3643507485ea2e06382d3f0b8a3
    release_profiles:
      - trash_ids: [76e060895c5b8a765c310933da0a5357] # Optionals
        filter:
          include:
            - 436f5a7d08fbf02ba25cb5e5dfe98e55 # Ignore Dolby Vision without HDR10 fallback
            - f3f0f3691c6a1988d4a02963e69d11f2 # Ignore The Group -SCENE
```

[sonarrjson]: https://github.com/TRaSH-/Guides/tree/master/docs/json/sonarr
