#include "pch.h"
#include <time.h>
#include <stdio.h>
#include "mkl.h"
#include "mkl_df_types.h"


extern "C"  _declspec(dllexport)
bool mkl_func(MKL_INT nx, MKL_INT dim, MKL_INT nRend, double* x, double* y, double* derivations, double* yRend, int& err)
{
	DFTaskPtr my_task;

	int res = dfdNewTask1D(&my_task, nx, x, DF_UNIFORM_PARTITION, dim, y, DF_MATRIX_STORAGE_ROWS);
	if (res != DF_STATUS_OK) {
		err = res;
		return false;
	}
	double* coef = new double[dim * 4 * (nx - 1)];
	res = dfdEditPPSpline1D(my_task, DF_PP_CUBIC, DF_PP_NATURAL,
		DF_BC_1ST_LEFT_DER | DF_BC_1ST_RIGHT_DER, derivations,
		DF_NO_IC, NULL, coef, DF_NO_HINT);
	if (res != DF_STATUS_OK) {
		err = res;
		return false;
	}

	res = dfdConstruct1D(my_task, DF_PP_SPLINE, DF_METHOD_STD);
	if (res != DF_STATUS_OK) {
		err = res;
		return false;
	}

	// shape(yRend) = [dim * nRend * 2]
	res = dfdInterpolate1D(my_task, DF_INTERP, DF_METHOD_PP, nRend, x,
		DF_UNIFORM_PARTITION, 2, new int[2]{ 1, 1 }, NULL, yRend, DF_MATRIX_STORAGE_ROWS, NULL);
	if (res != DF_STATUS_OK) {
		err = res;
		return false;
	}

	dfDeleteTask(&my_task);
	return true;
}
