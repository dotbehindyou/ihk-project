export const ADD_CUSTOMER = "ADD_CUSTOMER";
export const REM_CUSTOMER = "REM_CUSTOMER";

export function AddCustomer(newCustomer) {
  return {
    type: ADD_CUSTOMER,
    customer: newCustomer,
  };
}

export function RemoveCustomer(kdnr) {
  return {
    type: REM_CUSTOMER,
    kdnr,
  };
}
