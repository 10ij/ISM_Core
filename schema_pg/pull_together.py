"""Combine SQL files into one upload-friendly document."""

from __future__ import annotations

import argparse
import sys
from pathlib import Path


def sql_files(folder: Path) -> list[Path]:
    """Return all SQL files below *folder* in deterministic order."""
    return sorted(
        (path for path in folder.rglob("*.sql") if path.is_file()),
        key=lambda path: path.relative_to(folder).as_posix().lower(),
    )


def combine_sql(folder: Path) -> str:
    """Build one document containing every SQL file below *folder*."""
    files = sql_files(folder)
    sections: list[str] = [
        f"-- Combined SQL files from: {folder.resolve()}",
        f"-- Files included: {len(files)}",
        "",
    ]

    for path in files:
        relative_path = path.relative_to(folder).as_posix()
        sections.extend(
            [
                f"-- ==========================================================================",
                f"-- FILE: {relative_path}",
                f"-- ==========================================================================",
                path.read_text(encoding="utf-8").rstrip(),
                "",
            ]
        )

    return "\n".join(sections)


def main() -> int:
    parser = argparse.ArgumentParser(
        description="Print all .sql files in a folder as one combined document."
    )
    parser.add_argument(
        "folder",
        nargs="?",
        type=Path,
        default=Path(__file__).parent,
        help="Folder to search recursively (default: this script's folder).",
    )
    args = parser.parse_args()

    folder = args.folder.resolve()
    if not folder.is_dir():
        parser.error(f"Folder does not exist: {folder}")

    try:
        sys.stdout.write(combine_sql(folder))
        sys.stdout.write("\n")
    except UnicodeDecodeError as error:
        print(f"Could not read {error.object!r} as UTF-8.", file=sys.stderr)
        return 1
    except OSError as error:
        print(f"Could not read SQL files: {error}", file=sys.stderr)
        return 1

    return 0


if __name__ == "__main__":
    raise SystemExit(main())