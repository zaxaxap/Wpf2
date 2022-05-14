#include "pch.h"
#include <time.h>
#include <stdio.h>
#include "mkl.h"
#include "mkl_df_types.h"


extern "C"  _declspec(dllexport)
int global_func(MKL_INT nx, MKL_INT dim, MKL_INT nRend, double* x, double* y, double* derivations, double* yRend, int& ret)
{
	try 
	{
		DFTaskPtr task;

		int ret = dfdNewTask1D(&task, nx, x, DF_UNIFORM_PARTITION, dim, y, DF_MATRIX_STORAGE_ROWS);
		if (ret != DF_STATUS_OK) {
			return 0;
		}
		double* coef = new double[dim * 4 * (nx - 1)];
		ret = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_1ST_LEFT_DER | DF_BC_1ST_RIGHT_DER, derivations, DF_NO_IC, NULL, coef, DF_NO_HINT);
		if (ret != DF_STATUS_OK) {
			return 0;
		}

		ret = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
		if (ret != DF_STATUS_OK) {
			return 0;
		}
		ret = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, nRend, x, DF_UNIFORM_PARTITION, 2, new int[2]{ 1, 1 }, NULL, yRend, DF_MATRIX_STORAGE_ROWS, NULL);
		if (ret != DF_STATUS_OK) {
			return 0;
		}

		ret = dfDeleteTask(&task);
		if (ret != DF_STATUS_OK) {
			return 0;
		}
		return 1;
	}
	catch (...) {
		return 0;
	}

}
