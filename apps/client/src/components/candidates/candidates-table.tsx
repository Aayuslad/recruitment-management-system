import {
    type ColumnDef,
    type ColumnFiltersState,
    flexRender,
    getCoreRowModel,
    getFilteredRowModel,
    getPaginationRowModel,
    getSortedRowModel,
    type SortingState,
    useReactTable,
    type VisibilityState,
} from '@tanstack/react-table';
import { ArrowUpDown, ChevronDown } from 'lucide-react';
import * as React from 'react';

import { useGetCandidates } from '@/api/candidate-api';
import { Button } from '@/components/ui/button';
import {
    DropdownMenu,
    DropdownMenuCheckboxItem,
    DropdownMenuContent,
    DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { Input } from '@/components/ui/input';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import type { CandidateSummary } from '@/types/candidate-types';
import { useNavigate } from 'react-router-dom';
import { Spinner } from '../ui/spinner';
import { Checkbox } from '../ui/checkbox';
import { ApplyToJobOpeningDialog } from './internal/apply-to-job-opening-dialog';
import { useAccessChecker } from '@/hooks/use-has-access';

export function CandidatesTable() {
    const navigate = useNavigate();
    const [sorting, setSorting] = React.useState<SortingState>([]);
    const [columnFilters, setColumnFilters] =
        React.useState<ColumnFiltersState>([]);
    const [columnVisibility, setColumnVisibility] =
        React.useState<VisibilityState>({});
    const [rowSelection, setRowSelection] = React.useState({});
    const canAccess = useAccessChecker();

    const { data, isLoading, isError } = useGetCandidates();

    const columns: ColumnDef<CandidateSummary>[] = [
        ...(canAccess(['Admin', 'Recruiter'])
            ? ([
                  {
                      id: 'select',
                      header: ({ table }) => (
                          <Checkbox
                              checked={
                                  table.getIsAllPageRowsSelected() ||
                                  (table.getIsSomePageRowsSelected() &&
                                      'indeterminate')
                              }
                              onCheckedChange={(value) =>
                                  table.toggleAllPageRowsSelected(!!value)
                              }
                              aria-label="Select all"
                          />
                      ),
                      cell: ({ row }) => (
                          <Checkbox
                              checked={row.getIsSelected()}
                              onCheckedChange={(value) =>
                                  row.toggleSelected(!!value)
                              }
                              onClick={(e) => e.stopPropagation()}
                              aria-label="Select row"
                          />
                      ),
                      enableSorting: false,
                      enableHiding: false,
                  },
              ] as ColumnDef<CandidateSummary>[])
            : []),
        {
            accessorKey: 'name',
            header: () => <div className="w-[220px] ml-4">Name</div>,
            cell: ({ row }) => (
                <div className="font-medium ml-4">
                    {`${row.original.firstName} ${row.original.middleName} ${row.original.lastName}`}
                </div>
            ),
        },
        {
            accessorKey: 'collegeName',
            header: () => <div className="w-[220px]">College</div>,
            cell: ({ row }) => <div>{row.original.collegeName}</div>,
        },
        {
            id: 'gender',
            header: 'Gender',
            cell: ({ row }) => (
                <div className="font-medium w-[80px]">
                    {row.original.gender}
                </div>
            ),
        },
        {
            id: 'bg-verfied',
            header: 'BG Verified',
            cell: ({ row }) => (
                <div className="font-medium w-[100px]">
                    {row.original.isBgVerificationCompleted ? 'Yes' : 'No'}
                </div>
            ),
        },
        {
            id: 'created-at',
            header: ({ column }) => {
                return (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            column.toggleSorting(column.getIsSorted() === 'asc')
                        }
                    >
                        Created At
                        <ArrowUpDown />
                    </Button>
                );
            },
            cell: ({ row }) => (
                <div className="font-medium w-[160px]">
                    {new Date(row.original.createdAt).toLocaleString()}
                </div>
            ),
        },
    ];

    const table = useReactTable({
        data: data as CandidateSummary[],
        columns,
        onSortingChange: setSorting,
        onColumnFiltersChange: setColumnFilters,
        getCoreRowModel: getCoreRowModel(),
        getPaginationRowModel: getPaginationRowModel(),
        getSortedRowModel: getSortedRowModel(),
        getFilteredRowModel: getFilteredRowModel(),
        onColumnVisibilityChange: setColumnVisibility,
        onRowSelectionChange: setRowSelection,
        state: {
            sorting,
            columnFilters,
            columnVisibility,
            rowSelection,
        },
    });

    if (isLoading)
        return (
            <div className="h-[50vh] flex justify-center items-center">
                <Spinner className="size-8" />
            </div>
        );
    if (isError) return <div>Error Loading Candidates</div>;

    return (
        <div className="w-full">
            <h2 className="font-semibold text-xl">Candidates:</h2>

            {/* header */}
            <div className="flex items-center justify-between py-4">
                <Input
                    placeholder="Filter candidates with name..."
                    value={
                        (table.getColumn('name')?.getFilterValue() as string) ??
                        ''
                    }
                    onChange={(event) =>
                        table
                            .getColumn('name')
                            ?.setFilterValue(event.target.value)
                    }
                    className="max-w-sm"
                />
                <div className="flex items-center gap-2">
                    <ApplyToJobOpeningDialog
                        selectedCandidates={table
                            .getSelectedRowModel()
                            .rows.map((row) => ({ id: row.original.id }))}
                        visibleTo={['Admin', 'Recruiter']}
                    />
                    <DropdownMenu>
                        <DropdownMenuTrigger asChild>
                            <Button variant="outline" className="ml-auto">
                                Columns <ChevronDown className="ml-2 h-4 w-4" />
                            </Button>
                        </DropdownMenuTrigger>
                        <DropdownMenuContent align="end">
                            {table
                                .getAllColumns()
                                .filter((column) => column.getCanHide())
                                .map((column) => (
                                    <DropdownMenuCheckboxItem
                                        key={column.id}
                                        className="capitalize"
                                        checked={column.getIsVisible()}
                                        onCheckedChange={(value) =>
                                            column.toggleVisibility(!!value)
                                        }
                                    >
                                        {column.id}
                                    </DropdownMenuCheckboxItem>
                                ))}
                        </DropdownMenuContent>
                    </DropdownMenu>
                </div>
            </div>

            {/* table */}
            <div className="overflow-hidden rounded-md border">
                <Table>
                    <TableHeader>
                        {table.getHeaderGroups().map((headerGroup) => (
                            <TableRow key={headerGroup.id}>
                                {headerGroup.headers.map((header) => (
                                    <TableHead key={header.id}>
                                        {(() => {
                                            if (header.isPlaceholder) {
                                                return null;
                                            }
                                            return flexRender(
                                                header.column.columnDef.header,
                                                header.getContext()
                                            );
                                        })()}
                                    </TableHead>
                                ))}
                            </TableRow>
                        ))}
                    </TableHeader>
                    <TableBody>
                        {table.getRowModel().rows?.length ? (
                            table.getRowModel().rows.map((row) => (
                                <TableRow
                                    key={row.id}
                                    onClick={() =>
                                        navigate(
                                            `/candidates/candidate/${row.original.id}`
                                        )
                                    }
                                    data-state={
                                        row.getIsSelected() && 'selected'
                                    }
                                >
                                    {row.getVisibleCells().map((cell) => (
                                        <TableCell key={cell.id}>
                                            {flexRender(
                                                cell.column.columnDef.cell,
                                                cell.getContext()
                                            )}
                                        </TableCell>
                                    ))}
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell
                                    colSpan={columns.length}
                                    className="h-24 text-center"
                                >
                                    No Candidates Found.
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </div>

            {/* footer */}
            <div className="flex items-center justify-end space-x-2 py-4">
                <div className="space-x-2">
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={() => table.previousPage()}
                        disabled={!table.getCanPreviousPage()}
                    >
                        Previous
                    </Button>
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={() => table.nextPage()}
                        disabled={!table.getCanNextPage()}
                    >
                        Next
                    </Button>
                </div>
            </div>
        </div>
    );
}
