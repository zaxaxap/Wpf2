#pragma once
#include "mkl.h"

extern "C"  _declspec(dllexport)
bool mkl_func(MKL_INT nx, MKL_INT dim, MKL_INT nRend, double* x, double* y, double* derivations, double* yRend, int& err);