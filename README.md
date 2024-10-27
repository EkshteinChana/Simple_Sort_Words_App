# Sorting Words App

## Overview
The `SortWords.exe` program is designed to read a text file (F1), sort the words it contains lexicographically, and write the sorted, unique words to a new file (F2). Additionally, the program provides functionality to identify the most frequent word in the input file.

## Features
- Reads from a specified input file (F1).
- Sorts words in ascending or descending order based on user input.
- Ignores numeric values when sorting and outputting.
- Outputs unique words to the output file (F2).
- Displays the most frequent word along with its count from the input file. If there is more than one word with the highest frequency, only the first one encountered in the text will be displayed, along with its count.

## Usage

### Command Line
To use the program, compile it and run it from the command line with the input file path as an argument:

```bash
SortWords.exe "path_to_F1.txt"
