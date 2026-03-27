/* eslint-disable no-param-reassign */

import { Schema, Document, Model, PopulateOptions } from 'mongoose';

interface QueryOptions {
  sortBy?: string;
  populate?: string;
  limit?: string | number;
  page?: string | number;
}

interface PaginateOptions {
  results: Document[];
  page: number;
  limit: number;
  totalPages: number;
  totalResults: number;
}

interface SearchFields {
  [key: string]: string;
}

interface CustomPopulateOptions {
  path: string;
  populate?: CustomPopulateOptions;
}

const paginate = (schema: any) => {
  schema.statics.paginate = async function (
    filter: Record<string, any>,
    options: QueryOptions,
    searchFields: SearchFields = {}
  ): Promise<PaginateOptions> {
    try {
      // Xử lý tìm kiếm gần đúng theo nhiều trường bằng regex
      if (Object.keys(searchFields).length > 0) {
        for (const [field, value] of Object.entries(searchFields)) {
          if (value) {
            filter[field] = { $regex: value, $options: 'i' }; // Thêm điều kiện tìm kiếm
          }
        }
      }

      // Xử lý sắp xếp
      let sort = options.sortBy
        ? options.sortBy.split(',').map((sortOption) => {
          const [key, order] = sortOption.split(':');
          return (order === 'desc' ? '-' : '') + key;
        }).join(' ')
        : '-createdAt'; // Mặc định sắp xếp theo createdAt giảm dần

      // Xử lý phân trang
      const limit = options.limit && parseInt(options.limit as string, 10) > 0 ? parseInt(options.limit as string, 10) : 10;
      const page =
        options.page && !isNaN(parseInt(options.page as string, 10)) && parseInt(options.page as string, 10) >= -1
          ? parseInt(options.page as string, 10)
          : 1;

      let docsQuery = this.find(filter).sort(sort).lean();

      // Populate các trường liên quan (nếu có)
      if (options.populate) {
        options.populate.split(',').forEach((populateOption) => {
          docsQuery = docsQuery.populate(
            populateOption
              .split('.')
              .reverse()
              .reduce<CustomPopulateOptions>((a, b) => ({ path: b, populate: a }), undefined as unknown as CustomPopulateOptions)
          );
        });
      }

      if (page === -1) {
        // Trả về toàn bộ kết quả mà không phân trang
        const results = await docsQuery;
        return {
          results,
          page: -1,
          limit: results.length,
          totalPages: 1,
          totalResults: results.length,
        };
      }

      const skip = (page - 1) * limit;

      // Đếm tổng số bản ghi phù hợp
      const countPromise = this.countDocuments(filter).exec();

      // Thêm skip và limit nếu page > -1
      docsQuery = docsQuery.skip(skip).limit(limit);

      // Thực hiện truy vấn và trả về kết quả
      const [totalResults, results] = await Promise.all([countPromise, docsQuery]);
      const totalPages = Math.ceil(totalResults / limit);

      return {
        results,
        page,
        limit,
        totalPages,
        totalResults,
      };
    } catch (error) {
      throw new Error(`Error while paginating: ${(error as Error).message}`);
    }
  };
};

export default paginate;
