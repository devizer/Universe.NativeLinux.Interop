# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 3.16

# Delete rule output on recipe failure.
.DELETE_ON_ERROR:


#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:


# Remove some rules from gmake that .SUFFIXES does not remove.
SUFFIXES =

.SUFFIXES: .hpux_make_needs_suffix_list


# Suppress display of executed commands.
$(VERBOSE).SILENT:


# A target that is always out of date.
cmake_force:

.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /other-apps/clion/bin/cmake/linux/bin/cmake

# The command to remove a file.
RM = /other-apps/clion/bin/cmake/linux/bin/cmake -E remove -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/cmake-build-debug

# Include any dependencies generated for this target.
include CMakeFiles/so_src.dir/depend.make

# Include the progress variables for this target.
include CMakeFiles/so_src.dir/progress.make

# Include the compile flags for this target's objects.
include CMakeFiles/so_src.dir/flags.make

CMakeFiles/so_src.dir/library.c.o: CMakeFiles/so_src.dir/flags.make
CMakeFiles/so_src.dir/library.c.o: ../library.c
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/cmake-build-debug/CMakeFiles --progress-num=$(CMAKE_PROGRESS_1) "Building C object CMakeFiles/so_src.dir/library.c.o"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -o CMakeFiles/so_src.dir/library.c.o   -c /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/library.c

CMakeFiles/so_src.dir/library.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/so_src.dir/library.c.i"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/library.c > CMakeFiles/so_src.dir/library.c.i

CMakeFiles/so_src.dir/library.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/so_src.dir/library.c.s"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -S /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/library.c -o CMakeFiles/so_src.dir/library.c.s

CMakeFiles/so_src.dir/getdelays.c.o: CMakeFiles/so_src.dir/flags.make
CMakeFiles/so_src.dir/getdelays.c.o: ../getdelays.c
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/cmake-build-debug/CMakeFiles --progress-num=$(CMAKE_PROGRESS_2) "Building C object CMakeFiles/so_src.dir/getdelays.c.o"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -o CMakeFiles/so_src.dir/getdelays.c.o   -c /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/getdelays.c

CMakeFiles/so_src.dir/getdelays.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/so_src.dir/getdelays.c.i"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/getdelays.c > CMakeFiles/so_src.dir/getdelays.c.i

CMakeFiles/so_src.dir/getdelays.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/so_src.dir/getdelays.c.s"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -S /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/getdelays.c -o CMakeFiles/so_src.dir/getdelays.c.s

# Object files for target so_src
so_src_OBJECTS = \
"CMakeFiles/so_src.dir/library.c.o" \
"CMakeFiles/so_src.dir/getdelays.c.o"

# External object files for target so_src
so_src_EXTERNAL_OBJECTS =

libso_src.so: CMakeFiles/so_src.dir/library.c.o
libso_src.so: CMakeFiles/so_src.dir/getdelays.c.o
libso_src.so: CMakeFiles/so_src.dir/build.make
libso_src.so: CMakeFiles/so_src.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --bold --progress-dir=/home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/cmake-build-debug/CMakeFiles --progress-num=$(CMAKE_PROGRESS_3) "Linking C shared library libso_src.so"
	$(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/so_src.dir/link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
CMakeFiles/so_src.dir/build: libso_src.so

.PHONY : CMakeFiles/so_src.dir/build

CMakeFiles/so_src.dir/clean:
	$(CMAKE_COMMAND) -P CMakeFiles/so_src.dir/cmake_clean.cmake
.PHONY : CMakeFiles/so_src.dir/clean

CMakeFiles/so_src.dir/depend:
	cd /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/cmake-build-debug && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/cmake-build-debug /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/cmake-build-debug /home/user/CLionProjects/Universe.NativeLinux.Interop/so-src/cmake-build-debug/CMakeFiles/so_src.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : CMakeFiles/so_src.dir/depend

